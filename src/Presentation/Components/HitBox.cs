using System.Collections.Generic;
using Godot;

namespace CombatLab.Presentation.Components;

[GlobalClass]
public partial class HitBox : Area2D
{
    [ExportGroup("Settings")]
    [Export] public bool IsContact { get; set; } = false;
    [Export] public bool IsAttacking { get; set; } = false;
    
    public float Damage;
    [Export] public Node2D AttackerSource;

    
    private HashSet<ulong> _hitVictims = new HashSet<ulong>();
    public override void _Ready()
    {
        if (AttackerSource == null)
        {
            AttackerSource = this;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!IsContact)
        {
            if (!Monitorable && _hitVictims.Count > 0)
            {
                _hitVictims.Clear();
            }
        }
    }
    
    public bool TryHit(Node victim)
    {
        if (IsContact)
        {
            // Contact damage — always hits, cooldown is managed by HurtBox.InvincibilityTime
            return true; 
        }
        if (!Monitorable) return false;

        var victimId = victim.GetInstanceId();
        
        if (_hitVictims.Contains(victimId))
        {
            return false;
        }
        
        _hitVictims.Add(victimId);
        return true;
    }
    
    public bool IsActive()
    {
        return Monitorable; 
    }
    
    public Vector2 GetSourcePosition()
    {
        return AttackerSource.GlobalPosition;
    }
}