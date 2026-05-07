using Godot;

namespace CombatLab.entities.player.States;

[GlobalClass]
public partial class PlayerStateMachine : StateMachine<Player>
{
    protected override void RegisterTransitions()
    {
        _transitions.Add((nameof(PlayerIdle), "damaged"), nameof(PlayerHurt));
        _transitions.Add((nameof(PlayerIdle), "moved"), nameof(PlayerMove));
        _transitions.Add((nameof(PlayerIdle), "airborne"), nameof(PlayerAir));
        _transitions.Add((nameof(PlayerHurt), "stopped"), nameof(PlayerIdle));
        _transitions.Add((nameof(PlayerHurt), "stoppedAirborne"), nameof(PlayerAir));
        _transitions.Add((nameof(PlayerMove), "damaged"), nameof(PlayerHurt));
        _transitions.Add((nameof(PlayerMove), "stopped"), nameof(PlayerIdle));
        _transitions.Add((nameof(PlayerMove), "airborne"), nameof(PlayerAir));
        _transitions.Add((nameof(PlayerAir), "damaged"), nameof(PlayerHurt));
        _transitions.Add((nameof(PlayerAir), "moved"), nameof(PlayerMove));
        _transitions.Add((nameof(PlayerAir), "stopped"), nameof(PlayerIdle));
    }
}