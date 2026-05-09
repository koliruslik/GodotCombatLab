using CombatLab.Core.Interfaces;
using Godot;

namespace CombatLab.Presentation.Components;

[GlobalClass]
public partial class HurtBox : Node
{
    [Export] public Node Owner;
    [Export] public float InvincibilityTime = 0.5f;
    
    [Export] public ShapeCast2D ShapeCast;
    private IDamageable _damageable;
    private float _cooldownTimer = 0.0f;

    public override void _Ready()
    {
        if (ShapeCast == null)
        {
            GD.PushError("No shape cast found");
        }
        if (Owner is IDamageable damageable) 
            _damageable = damageable;
        else
            GD.PushError("Owner must implement IDamageable");
        
    }
    public override void _PhysicsProcess(double delta)
    {
        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= (float)delta;
            return;
        }
        
        if (ShapeCast == null || _damageable == null) return;
        
        if (ShapeCast.IsColliding())
        {
            for (int i = 0; i < ShapeCast.GetCollisionCount(); i++)
            {
                var collider = ShapeCast.GetCollider(i);
                
                if (collider is HitBox hitbox)
                {
                    if (hitbox.TryHit(Owner)) 
                    {
                        var attackerPos = hitbox.GetSourcePosition();
                        _damageable.TakeDamage(hitbox.Damage, attackerPos);
                        _cooldownTimer = InvincibilityTime;
                    }
                }
            }
        }
    }
}
