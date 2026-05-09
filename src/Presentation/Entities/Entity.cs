using CombatLab.Core.Interfaces;
using Godot;

namespace CombatLab.Presentation.Entities;

public abstract partial class Entity : CharacterBody2D, ICombatant
{
    public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    public abstract void TakeDamage(int damage, Vector2 sourcePosition);
}