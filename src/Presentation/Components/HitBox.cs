using System.Collections.Generic;
using Godot;

namespace CombatLab.Presentation.Components;

[GlobalClass]
public partial class HitBox : Area2D
{
    [Signal] public delegate void HitDetectedEventHandler(Node victim);
    
    [Export] public Node Owner;
    //[Export] public ShapeCast2D ShapeCast2D;

    private HashSet<ulong> _hitVictims = new();

    public override void _PhysicsProcess(double delta)
    {
        foreach (var area in GetOverlappingAreas())
        {
            //GD.Print($"Overlapping: {area.Name}");
            if (area is HitBox other && other.Owner != Owner)
            {
                //GD.Print($"HitBox found: {other.Owner?.Name}");
                var id = other.Owner.GetInstanceId();
                if (_hitVictims.Contains(id)) continue;
                _hitVictims.Add(id);
                EmitSignal(SignalName.HitDetected, other.Owner);
            }
        }
    }

    public void ResetHits()
    {
        _hitVictims.Clear();
    }

}

// public partial class HitBox : Area2D
// {
//     [ExportGroup("Settings")]
//     [Export] public bool IsContact { get; set; } = false;
//     [Export] public bool IsAttacking { get; set; } = false;
//     
//     public float Damage;
//     [Export] public Node2D AttackerSource;
//
//     
//     private HashSet<ulong> _hitVictims = new HashSet<ulong>();
//     public override void _Ready()
//     {
//         if (AttackerSource == null)
//         {
//             AttackerSource = this;
//         }
//     }
//
//     public override void _PhysicsProcess(double delta)
//     {
//         if (!IsContact)
//         {
//             if (!Monitorable && _hitVictims.Count > 0)
//             {
//                 _hitVictims.Clear();
//             }
//         }
//     }
//     
//     public bool TryHit(Node victim)
//     {
//         if (IsContact)
//         {
//             // Contact damage — always hits, cooldown is managed by HurtBox.InvincibilityTime
//             return true; 
//         }
//         if (!Monitorable) return false;
//
//         var victimId = victim.GetInstanceId();
//         
//         if (_hitVictims.Contains(victimId))
//         {
//             return false;
//         }
//         
//         _hitVictims.Add(victimId);
//         return true;
//     }
//     
//     public bool IsActive()
//     {
//         return Monitorable; 
//     }
//     
//     public Vector2 GetSourcePosition()
//     {
//         return AttackerSource.GlobalPosition;
//     }
// }