using Godot;

namespace CombatLab.Core.Interfaces;

public interface IDamageable
{
    void TakeDamage(int damage, Vector2 sourcePosition);
}