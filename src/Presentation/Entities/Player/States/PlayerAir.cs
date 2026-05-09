using Godot;
using GodotCombatLab.Core.FSM;

namespace CombatLab.Presentation.Entities.Player.States;

[GlobalClass]
public partial class PlayerAir : State<Player>
{
    public override void Enter()
    {
        GD.Print("Entering PlayerAir");
        if (Actor.Velocity.Y < 0)
            Actor.TravelToAnimation("jump");
        else
            Actor.TravelToAnimation("fall");
    }

    public override void Update(double delta)
    {
        Actor.Controller.TryAttack();
    }
    public override void PhysicsUpdate(double delta)
    {
        float moveInput = Actor.Input.MoveDirection.X;
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