using Godot;
using GodotCombatLab.Core.FSM;

namespace CombatLab.Presentation.Entities.Player.States;

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
        {
            Actor.Controller.Jump();
            return;
        }
    }
    
    public override void PhysicsUpdate(double delta)
    {
        float moveInput = Actor.Input.MoveDirection.X;
        
        if (Mathf.IsZeroApprox(moveInput) && Mathf.IsZeroApprox(Actor.Velocity.X))
        {
            EmitSignal(SignalName.Transitioned, this, "stopped");
            return;
        }
        Actor.Controller.ApplyMovement(moveInput, delta);
        
        if (!Actor.IsOnFloor()) 
        {
            EmitSignal(SignalName.Transitioned, this, "airborne");
        }
    }
}