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
        if (Actor.Input.IsJumpJustPressed && Actor.IsOnFloor()) 
            Actor.Jump();
    }
 
    public override void PhysicsUpdate(double delta)
    {
        Actor.ApplyGravity(delta);
        Actor.HandleMovement(0, Actor.Speed * 5, delta);
        
        if (!Actor.IsOnFloor()) EmitSignal(SignalName.Transitioned, this, "playerair");
        else if (Actor.Input.MoveDirection.X != 0) EmitSignal(SignalName.Transitioned, this, "playermove");
    }
    
}