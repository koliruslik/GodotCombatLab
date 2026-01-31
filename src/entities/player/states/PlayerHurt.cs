using Godot;
namespace CombatLab.entities.player.States;

[GlobalClass]
public partial class PlayerHurt : State<Player>
{
    [Export] public float StunDuration = 0.3f;
    [Export] public float KnockbackStrength = 300f; 
    [Export] public float KnockbackLift = -200f;

    private float _timer;

    public override void Enter()
    {
        GD.Print("Entering PlayerHurt State");
        
        _timer = StunDuration;
        Actor.TravelToAnimation("hurt");

        float knockbackForce = 300f;
        Actor.Velocity = Actor.KnockbackDirection * knockbackForce;
    }

    public override void PhysicsUpdate(double delta)
    {
        Actor.ApplyMovement(0, delta);
        
        _timer -= (float)delta;

        if (_timer <= 0)
        {
            if (Actor.IsOnFloor())
                EmitSignal(SignalName.Transitioned, this, "playeridle");
            else
                EmitSignal(SignalName.Transitioned, this, "playerair");
        }
    }
    
}