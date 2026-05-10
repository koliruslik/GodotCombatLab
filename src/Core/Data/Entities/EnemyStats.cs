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
    
    [ExportGroup("Stats")]
    [Export] public float MaxHP;
    [Export] public float MaxMP;
    [Export] public float Damage;
    [Export] public float Defense;
    [Export] public float Speed;
    [Export] public float JumpVelocity;
    
    [ExportGroup("Weapons")]
    [Export] public WeaponData WeaponData; 
}