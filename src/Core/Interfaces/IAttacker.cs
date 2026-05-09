using Godot;

namespace CombatLab.Core.Interfaces;

public interface IAttacker
{
    Vector2 GlobalPosition { get; }
}