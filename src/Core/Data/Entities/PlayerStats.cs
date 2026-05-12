using Godot;

namespace CombatLab.Core.Data.Entities;

[GlobalClass]
public partial class PlayerStats : Resource
{
    [Export] public float MaxHP;
    [Export] public float MaxMP;
    [Export] public float Defense;
    [Export] public float Speed;
    [Export] public float Friction;
    [Export] public float Acceleration;
    [Export] public float JumpVelocity;
    [Export] public float InvincibleTime;
}