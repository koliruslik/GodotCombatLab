using CombatLab.Core.Interfaces;
using CombatLab.Presentation.Components;
using Godot;

namespace CombatLab.Presentation.Entities;

public abstract partial class Entity : CharacterBody2D, IAttacker, IDamageable
{
    [Export] public HealthComponent Health;
    public float Gravity;

    public override void _Ready()
    {
        if(Health == null) { GD.Print("Entity has no health component"); return; }
        Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    }
    public void TakeDamage(float damage, Vector2 sourcePosition)
        => Health.TakeDamage(damage, sourcePosition);
}