using Godot;

namespace CombatLab.Core.Data;

public partial class WeaponData : ItemData
{
    [Export] public float Damage;
    [Export] public float Range;
    [Export] public float MageDamage;
}

public enum AttackStrategy
{
    Melee,
    Ranged,
    Mage,
    Special
}