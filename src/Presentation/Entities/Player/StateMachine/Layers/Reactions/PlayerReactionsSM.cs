using CombatLab.Core.FSM;
using CombatLab.Presentation.Entities.Player.StateMachine.States;
using Godot;

namespace CombatLab.Presentation.Entities.Player.StateMachine.Layers;

[GlobalClass]
public partial class PlayerReactionsSM : StateMachine<Player>
{
    protected override void RegisterTransitions()
    {
        _transitions.Add((nameof(PlayerEmpty), "damaged"), nameof(PlayerHurt));
        _transitions.Add((nameof(PlayerHurt), "finished"), nameof(PlayerEmpty));
        _transitions.Add((nameof(PlayerEmpty), "died"), nameof(PlayerDie));
        _transitions.Add((nameof(PlayerDie), "finished"), nameof(PlayerEmpty));
    }
}