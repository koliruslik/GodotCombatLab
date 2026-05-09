using CombatLab.Core.Interfaces;
using Godot;

namespace CombatLab.Presentation.Components;

[GlobalClass]
public partial class HurtBox : Node2D
{
    [Export] public Node OwnerNode;
    [Export] public float InvincibilityTime = 0.5f;
    
    [Export] public ShapeCast2D ShapeCast;
    private IDamageable _damageable;
    private float _cooldownTimer = 0.0f;

    public override void _Ready()
    {
        if (OwnerNode is IDamageable damageable) _damageable = damageable;
        
        if (ShapeCast == null)
        {
            GD.PushError("No shape cast found");
        }
        
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
                    if (hitbox.TryHit(OwnerNode)) 
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
