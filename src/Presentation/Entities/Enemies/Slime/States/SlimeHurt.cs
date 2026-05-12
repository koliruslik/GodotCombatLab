using CombatLab.Core.FSM;
using CombatLab.Core.Utils;
using Godot;

namespace CombatLab.Presentation.Entities.Enemies.SlimeStates;

[GlobalClass]
public partial class SlimeHurt : State<Slime>
{
    [Export] public float HurtStateDuration = 0.1f;
    private float _timer = 0f;
    
    public override void Enter()
    {
        _timer = HurtStateDuration;
        GameLogger.Debug($"Timer: {_timer}, HurtStateDuration: {HurtStateDuration}", LogCategory.State);
        GameLogger.Debug($"Entering {StateName}", LogCategory.State); 
        Actor.Sprite.Modulate = Colors.Red;
        var knockbackDir = (Actor.GlobalPosition - Actor.LastHitSourcePosition).Normalized();
        Actor.Velocity = knockbackDir * Actor.LastKnockbackForce;
    }

    public override void PhysicsUpdate(double delta)
    {
        _timer -= (float)delta;
        if(_timer > 0f) return;
        Actor.Sprite.Modulate = Colors.White;
        EmitSignal(SignalName.Transitioned, this, "gotHit");
    }
}