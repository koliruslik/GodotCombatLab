using Godot;

namespace CombatLab.Core.Interfaces;

public interface IDamageable
{
    void TakeDamage(float damage, Vector2 sourcePosition);
}