using CombatLab.Core.FSM;
using CombatLab.Core.Utils;
using Godot;

namespace CombatLab.Presentation.Entities.Player.StateMachine.States;

[GlobalClass]
public partial class PlayerDie : State<Player>
{
    public override void Enter()
    {
        base.Enter();
        Actor.PlayAnimation("die");
        Actor.Health.InvincibilityTime = float.PositiveInfinity;
        Actor.Sprite.AnimationFinished += OnDieFinished;
    }
    
    public override void Exit()
    {
        base.Exit();
    }

    private void OnDieFinished()
    {
        Actor.Sprite.AnimationFinished -= OnDieFinished;
        Actor.Die();
    }
    
}