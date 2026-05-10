using System.Collections.Generic;
using Godot;

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