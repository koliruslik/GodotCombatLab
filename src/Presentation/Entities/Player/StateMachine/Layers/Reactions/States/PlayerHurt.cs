using Godot;
using CombatLab.Core.FSM;
using CombatLab.Core.Utils;

namespace CombatLab.Presentation.Entities.Player.StateMachine.States;

[GlobalClass]
public partial class PlayerHurt : State<Player>
{
    [Export] public float StunDuration = 0.3f;
    [Export] public float KnockbackStrength = 300f; 
    [Export] public float KnockbackLift = -200f;

    private float _timer;

    public override void Enter()
    {
        GameLogger.Debug("Entering PlayerHurt State", LogCategory.State);
        _timer = StunDuration;
        Actor.TravelToAnimation("hurt");
        Actor.Velocity = Actor.KnockbackDirection *  KnockbackStrength;
    }

    public override void PhysicsUpdate(double delta)
    {
        _timer -= (float)delta;
        if (_timer <= 0)
            EmitSignal(SignalName.Transitioned, this, "finished");
    }
    
}