using CombatLab.Core.Interfaces;
using CombatLab.Presentation.Strategies.Attack;

namespace CombatLab.Core.Data.Items.Weapons;

public enum WeaponType
{
    Melee,
    Ranged,
    Mage,
    Special
}

public static class WeaponTypeExtensions
{
    public static IAttackStrategy ToStrategy(this WeaponType type, float damage) => type switch
    {
        WeaponType.Melee => new MeleeAttack(damage),
        WeaponType.Ranged => new RangedAttack(damage),
        WeaponType.Mage => new MageAttack(damage),
        _ => throw new System.NotImplementedException($"No strategy for WeaponType: {type}")
    };
    
    public static IAttackStrategy ToStrategy(this WeaponType type, float damage, float knockbackForce, float knockbackLift) => type switch
    {
        WeaponType.Melee => new MeleeAttack(damage, knockbackForce,  knockbackLift),
        WeaponType.Ranged => new RangedAttack(damage),
        WeaponType.Mage => new MageAttack(damage),
        _ => throw new System.NotImplementedException($"No strategy for WeaponType: {type}")
    };
}