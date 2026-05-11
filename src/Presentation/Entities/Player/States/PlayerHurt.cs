using Godot;
using CombatLab.Core.FSM;
using CombatLab.Core.Utils;

namespace CombatLab.Presentation.Entities.Player.States;

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

        float knockbackForce = 300f;
        Actor.Velocity = Actor.KnockbackDirection * knockbackForce;
    }

    public override void PhysicsUpdate(double delta)
    {
        Actor.Controller.ApplyMovement(0, delta);
        
        _timer -= (float)delta;

        if (_timer <= 0)
        {
            if (Actor.IsOnFloor())
                EmitSignal(SignalName.Transitioned, this, "stopped");
            else
                EmitSignal(SignalName.Transitioned, this, "stoppedAirborne");
        }
    }
    
}