using Godot;

namespace CombatLab.Core.Interfaces;

public interface ICombatant : IDamageable
{
    Vector2 GlobalPosition { get; }
}