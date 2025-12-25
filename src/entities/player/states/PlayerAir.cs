using Godot;

namespace CombatLab.entities.player.States;

[GlobalClass]
public partial class PlayerAir : State<Player>
{
    public override void PhysicsUpdate(double delta)
    {
        Actor.ApplyGravity(delta);
        var moveInput = Actor.Input.MoveDirection.X;
        
        Actor.HandleMovement(moveInput * Actor.Speed, Actor.Speed * 2, delta);
        Actor.UpdateFacing(moveInput);

        Actor.TravelToAnimation(Actor.Velocity.Y < 0 ? "jump" : "fall");
        if (Actor.IsOnFloor())
        {
            var nextState = Mathf.IsZeroApprox(moveInput) ? "playeridle" : "playermove";
            EmitSignal(SignalName.Transitioned, this, nextState);
        }
    }
}