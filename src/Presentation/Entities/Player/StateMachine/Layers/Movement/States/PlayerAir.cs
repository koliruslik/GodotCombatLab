using Godot;
using CombatLab.Core.FSM;
using CombatLab.Core.Utils;

namespace CombatLab.Presentation.Entities.Player.StateMachine.States;

[GlobalClass]
public partial class PlayerAir : State<Player>
{
    public override void Enter()
    {
        base.Enter();
        if (Actor.Velocity.Y < 0)
            Actor.PlayAnimation("jump");
        else
            Actor.PlayAnimation("fall");
    }

    public override void Refresh()
    {
        base.Refresh();
        if (Actor.Velocity.Y < 0)
            Actor.PlayAnimation("jump");
        else
            Actor.PlayAnimation("fall");
    }

    public override void Update(double delta)
    {
        
    }
    public override void PhysicsUpdate(double delta)
    {
        if (Actor.Lsm.IsBusy) return;
        float moveInput = Actor.PlayerInput.MoveDirection.X;
        Actor.Controller.ApplyMovement(moveInput, delta);
        
        if (Actor.Velocity.Y > 0)
        {
            Actor.PlayAnimation("fall");
        }

        if (Actor.IsOnFloor())
        {
            if (Actor.Velocity.Y >= 0)
            {
                if (!Mathf.IsZeroApprox(moveInput))
                {
                    EmitSignal(SignalName.Transitioned, this, "moved");
                }
                else
                {
                    EmitSignal(SignalName.Transitioned, this, "stopped");
                }
            }
        }
    }
}