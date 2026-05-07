using Godot;

namespace CombatLab.entities.components;

public interface IDamageable
{
    void TakeDamage(int damage, Vector2 sourcePosition);
}