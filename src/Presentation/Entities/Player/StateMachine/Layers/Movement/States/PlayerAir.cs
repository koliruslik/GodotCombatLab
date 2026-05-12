using Godot;
using CombatLab.Core.FSM;
using CombatLab.Core.Utils;

namespace CombatLab.Presentation.Entities.Player.StateMachine.States;

[GlobalClass]
public partial class PlayerAir : State<Player>
{
    public override void Enter()
    {
        GameLogger.Debug("Entering PlayerAir", LogCategory.State);
        if (Actor.Velocity.Y < 0)
            Actor.TravelToAnimation("jump");
        else
            Actor.TravelToAnimation("fall");
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
            Actor.TravelToAnimation("fall");
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