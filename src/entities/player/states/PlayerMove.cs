using Godot;

namespace CombatLab.entities.player.States;

[GlobalClass]
public partial class PlayerMove : State<Player>
{
    public override void Enter()
    {
        GD.Print("Entering PlayerMove");
        Actor.TravelToAnimation("walk");
    }

    public override void Update(double delta)
    {
        if (Actor.Input.IsJumpJustPressed && Actor.IsOnFloor()) 
            Actor.Jump();
    }
    
    public override void PhysicsUpdate(double delta)
    {
        Actor.ApplyGravity(delta);
        var moveInput = Actor.Input.MoveDirection.X;
    
        Actor.HandleMovement(moveInput * Actor.Speed, Actor.Speed * 8, delta);
        Actor.UpdateFacing(moveInput);
        
        if (!Actor.IsOnFloor()) EmitSignal(SignalName.Transitioned, this, "playerair");
        else if (Mathf.IsZeroApprox(moveInput)) EmitSignal(SignalName.Transitioned, this, "playeridle");
    }
}