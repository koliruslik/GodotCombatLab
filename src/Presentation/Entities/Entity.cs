using CombatLab.Core.Interfaces;
using Godot;

namespace CombatLab.Presentation.Entities;

public abstract partial class Entity : CharacterBody2D, ICombatant
{
    public float Speed = 10;
    public float Acceleration = 10;
    public float Friction = 0.5f;
    public float MaxHP = 100;
    public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    public abstract void TakeDamage(int damage, Vector2 sourcePosition);
}