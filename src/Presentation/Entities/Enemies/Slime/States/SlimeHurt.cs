using CombatLab.Core.FSM;
using CombatLab.Core.Utils;
using Godot;

namespace CombatLab.Presentation.Entities.Enemies.SlimeStates;

[GlobalClass]
public partial class SlimeHurt : State<Slime>
{
    public override void Enter()
    {
        GameLogger.Debug($"Entering {StateName}", LogCategory.State); 
        Actor.Sprite.Modulate = Colors.Red;
        var knockbackDir = (Actor.GlobalPosition - Actor.LastHitSourcePosition).Normalized();
        Actor.Velocity = knockbackDir * Actor.LastKnockbackForce;
        Actor.GetTree().CreateTimer(0.3f).Timeout += () =>
        {
            if (!IsInstanceValid(Actor)) return;
            EmitSignal(SignalName.Transitioned, this, "gotHit");
            Actor.Sprite.Modulate = Colors.White;
        };
    }
    
}