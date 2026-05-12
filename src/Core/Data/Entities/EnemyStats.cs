using CombatLab.Core.Data.Items.Weapons;
using CombatLab.Core.Interfaces;
using Godot;

namespace CombatLab.Core.Data.Entities;

[GlobalClass]
public partial class EnemyStats : Resource
{
    [ExportGroup("BaseStats")]
    [Export] public string Name;
    [Export] public int Gold;
    [Export] public float MaxHP;
    [Export] public float MaxMP;

    [ExportGroup("Combat")]
    [Export] public float AttackCooldown;
    [Export] public float Damage; 
    [Export] public WeaponData WeaponData; 
    [ExportGroup("Defense")]
    [Export] public float Defense;
    [Export] public float KnockbackDefence;
    
    [ExportGroup("Range")]
    [Export] public float DetectionRange;
    [Export] public float AttackRange;
    
    [ExportGroup("Movement")]
    [Export] public float Speed;
    [Export] public float JumpVelocity;
    
}