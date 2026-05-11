using Godot;
using CombatLab.Core.FSM;
using CombatLab.Core.Utils;

namespace CombatLab.Presentation.Entities.Player.States;

[GlobalClass]
public partial class PlayerIdle : State<Player>
{
    public override void Enter()
    {
        GameLogger.Debug("Entering PlayerIdle", LogCategory.State);
        Actor.TravelToAnimation("idle");
    }

    public override void Update(double delta)
    {
        if (Actor.PlayerInput.IsJumpJustPressed && Actor.IsOnFloor()) 
            Actor.Controller.Jump();
        if (!Mathf.IsZeroApprox(Actor.PlayerInput.MoveDirection.X))
            EmitSignal(SignalName.Transitioned, this, "moved");
    }
 
    public override void PhysicsUpdate(double delta)
    {
        Actor.Controller.ApplyMovement(0, delta);

        if (!Actor.IsOnFloor()) 
            EmitSignal(SignalName.Transitioned, this, "airborne");
    }
}