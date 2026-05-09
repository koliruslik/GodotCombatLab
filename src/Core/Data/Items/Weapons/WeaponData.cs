using Godot;

namespace CombatLab.Core.Data.Items.Weapons;

[GlobalClass]
public partial class WeaponData : ItemData
{
    [Export] public WeaponType WeaponType;
    // TODO: Replace with AttackPattern resource when implemented
    [Export] public Animation AttackPattern; 
    [Export] public float Damage;
}