using Godot;

namespace CombatLab.entities;

public partial class Entity : CharacterBody2D
{
    [Export] public float Speed = 300.0f;
    [Export] public float Acceleration = 10.0f;
    [Export] public float Friction = 200.0f;
    
    public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
}