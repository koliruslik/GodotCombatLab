using CombatLab.Core.FSM;
using Godot;

namespace CombatLab.Presentation.Entities.Enemies.SlimeStates;

[GlobalClass]
public partial class SlimeStateMachine : StateMachine<Slime>
{
    protected override void RegisterTransitions()
    {
        _transitions.Add((nameof(SlimeIdle), "playerSpotted"), nameof(SlimeChase));
        _transitions.Add((nameof(SlimeChase), "playerLost"), nameof(SlimeIdle));
        _transitions.Add((nameof(SlimeHurt), "gotHit"), nameof(SlimeChase));
    }

    public void OnHurt() => ChangeState(nameof(SlimeHurt));
    public void OnDie() => ChangeState(nameof(SlimeDie));
}