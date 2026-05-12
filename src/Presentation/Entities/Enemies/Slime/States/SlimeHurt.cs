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
        base.Enter();
        _timer = HurtStateDuration;
        Actor.Sprite.Modulate = Colors.Red;
        var dir = (Actor.GlobalPosition - Actor.LastHitSourcePosition).Normalized();
        Actor.Velocity = new Vector2(dir.X * Actor.LastKnockbackForce, -Actor.LastKnockbackLift);
    }

    public override void PhysicsUpdate(double delta)
    {
        _timer -= (float)delta;
        if(_timer > 0f) return;
        Actor.Sprite.Modulate = Colors.White;
        EmitSignal(SignalName.Transitioned, this, "gotHit");
    }
}