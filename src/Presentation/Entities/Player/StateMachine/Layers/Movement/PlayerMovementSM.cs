using CombatLab.Core.FSM;
using Godot;
using CombatLab.Presentation.Entities.Player.StateMachine.States;
namespace CombatLab.Presentation.Entities.Player.StateMachine.Layers;

[GlobalClass]
public partial class PlayerMovementSM : StateMachine<Player>
{
    protected override void RegisterTransitions()
    {
        _transitions.Add((nameof(PlayerIdle), "moved"), nameof(PlayerMove));
        _transitions.Add((nameof(PlayerIdle), "airborne"), nameof(PlayerAir));
        _transitions.Add((nameof(PlayerMove), "stopped"), nameof(PlayerIdle));
        _transitions.Add((nameof(PlayerMove), "airborne"), nameof(PlayerAir));
        _transitions.Add((nameof(PlayerAir), "moved"), nameof(PlayerMove));
        _transitions.Add((nameof(PlayerAir), "stopped"), nameof(PlayerIdle));
    }
}