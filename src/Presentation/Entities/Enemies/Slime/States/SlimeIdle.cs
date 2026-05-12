using CombatLab.Core.FSM;
using CombatLab.Core.Utils;
using Godot;

namespace CombatLab.Presentation.Entities.Enemies.SlimeStates;

[GlobalClass]
public partial class SlimeIdle : State<Slime>
{
    public override void Enter()
    {
        GameLogger.Debug($"Entering {StateName}", LogCategory.State); 
        Actor.PlayAnimation("idle");
    }

    public override void PhysicsUpdate(double delta)
    {
        GameLogger.Debug($"Distance: {Actor.GlobalPosition.DistanceTo(Actor.Player?.GlobalPosition ?? Vector2.Zero)}", LogCategory.Detailed);
        Actor.Velocity = new Vector2(
            Mathf.MoveToward(Actor.Velocity.X, 0, Actor.Stats.Speed),
            Actor.Velocity.Y
        );
        if (Actor.IsPlayerInDetecionRange())
            EmitSignal(SignalName.Transitioned, this, "playerSpotted");
    }
}