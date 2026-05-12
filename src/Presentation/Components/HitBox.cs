using System.Collections.Generic;
using Godot;
using CombatLab.Core.Utils;

namespace CombatLab.Presentation.Components;

[GlobalClass]
public partial class HitBox : Area2D
{
    [Signal] public delegate void HitDetectedEventHandler(Node victim);
    
    [Export] public Node Owner;

    private HashSet<ulong> _hitVictims = new();

    public override void _PhysicsProcess(double delta)
    {
        foreach (var area in GetOverlappingAreas())
        {
            GameLogger.Debug($"Overlapping: {area.Name}", LogCategory.Detailed);
            if (area is HitBox other && other.Owner != Owner)
            {
                var id = other.Owner.GetInstanceId();
                if (_hitVictims.Contains(id)) continue;
                _hitVictims.Add(id);
                GameLogger.Debug($"HitBox found: {other.Owner?.Name}", LogCategory.Combat);
                EmitSignal(SignalName.HitDetected, other.Owner);
            }
        }
    }

    public void ResetHits()
    {
        _hitVictims.Clear();
    }

}