using Godot;

namespace CombatLab.Core.Interfaces;

public interface IKnockbackable
{
    void ApplyKnockback(Vector2 sourcePosition, float force);
}