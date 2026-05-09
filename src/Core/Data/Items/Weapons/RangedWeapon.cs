using Godot;

namespace CombatLab.Core.Data.Items.Weapons;

[GlobalClass]
public partial class RangedWeapon : WeaponData
{
    [Export] public float Range;
    [Export] public ItemData Ammunition;
}