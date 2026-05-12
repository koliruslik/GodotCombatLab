using Godot;
using CombatLab.Core.FSM;
using CombatLab.Core.Utils;

namespace CombatLab.Presentation.Entities.Player.StateMachine.States;

[GlobalClass]
public partial class PlayerMove : State<Player>
{
    public override void Enter()
    {
        base.Enter();
        Actor.PlayAnimation("walk");
    }

    public override void Refresh()
    {
        base.Refresh();
        Actor.PlayAnimation("walk");
    }

    public override void Update(double delta)
    {
        if (Actor.Lsm.IsBusy) return;
        if (Actor.PlayerInput.IsJumpJustPressed && Actor.IsOnFloor()) 
        {
            Actor.Controller.Jump();
            return;
        }
    }
    
    public override void PhysicsUpdate(double delta)
    {
        if (Actor.Lsm.IsBusy) return;
        float moveInput = Actor.PlayerInput.MoveDirection.X;
        
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