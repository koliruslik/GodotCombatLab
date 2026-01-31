using Godot;

namespace CombatLab.entities.player.States;

[GlobalClass]
public partial class PlayerIdle : State<Player>
{
    public override void Enter()
    {
        GD.Print("Entering PlayerIdle");
        Actor.TravelToAnimation("idle");
    }

    public override void Update(double delta)
    {
        Actor.TryAttack();
        if (Actor.Input.IsJumpJustPressed && Actor.IsOnFloor()) 
            Actor.Jump();
        if (!Mathf.IsZeroApprox(Actor.Input.MoveDirection.X))
            EmitSignal(SignalName.Transitioned, this, "playermove");
    }
 
    public override void PhysicsUpdate(double delta)
    {
        Actor.ApplyMovement(0, delta);

        if (!Actor.IsOnFloor()) 
            EmitSignal(SignalName.Transitioned, this, "playerair");
    }
}