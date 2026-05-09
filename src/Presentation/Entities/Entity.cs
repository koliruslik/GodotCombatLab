using CombatLab.Core.Interfaces;
using Godot;

namespace CombatLab.Presentation.Entities;

public abstract partial class Entity : CharacterBody2D, IAttacker, IDamageable
{
    public float Gravity;

    public override void _Ready()
    {
        Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    }
    public abstract void TakeDamage(float damage, Vector2 sourcePosition);
}