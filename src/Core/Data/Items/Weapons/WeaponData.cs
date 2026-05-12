using Godot;

namespace CombatLab.Core.Data.Items.Weapons;

[GlobalClass]
public partial class WeaponData : ItemData
{
    [ExportGroup("Combat")]
    [Export] public WeaponType WeaponType;
    [Export] public float Damage;
    [Export] public float KnockbackForce;
    [Export] public float KnockbackLift;
    [Export] public float Range;
    // TODO: Replace with AttackPattern resource when implemented
    [Export] public Animation AttackPattern;

    [ExportGroup("Ranged")] 
    [Export] public PackedScene ProjectileScene;
    [Export] public  float ProjectileSpeed;
    
    [ExportGroup("Mage")] 
    [Export] public float MPCost;
    
    [ExportGroup("Special")]
    [Export] public string PLACEHOLDER;
    //not implemented yet
}