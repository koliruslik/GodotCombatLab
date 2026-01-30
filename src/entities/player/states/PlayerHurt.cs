using Godot;
namespace CombatLab.entities.player.States;

[GlobalClass]
public partial class PlayerHurt : State<Player>
{
    [Export] public float StunDuration = 0.3f;
    [Export] public Vector2 KnockbackSpeed = new Vector2(-300, -100);

    private float _timer;

    public override void Enter()
    {
        GD.Print("Entering PlayerHurt State");
        
        _timer = StunDuration;
        Actor.TravelToAnimation("hurt");

        float direction = Actor.Input.FacingRight ? -1 : 1;
        Actor.Velocity = new Vector2(KnockbackSpeed.X * direction, KnockbackSpeed.Y);
    }

    public override void PhysicsUpdate(double delta)
    {
        Actor.ApplyGravity(delta);
        Actor.HandleMovement(0, 500f, delta);
        
        _timer -= (float)delta;

        if (_timer <= 0)
        {
            if (!Actor.IsOnFloor())
                EmitSignal(SignalName.Transitioned, this, "playerair");
            else
                EmitSignal(SignalName.Transitioned, this, "playeridle");
        }
    }
    
}