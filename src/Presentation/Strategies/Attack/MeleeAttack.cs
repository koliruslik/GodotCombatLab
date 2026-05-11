using CombatLab.Core.Interfaces;
using Godot;
using CombatLab.Core.Utils;

namespace CombatLab.Presentation.Strategies.Attack;

public class MeleeAttack : IAttackStrategy
{
    private float _damage;
    public MeleeAttack(float damage) {_damage = damage;}
    public void Execute(IAttacker attacker, IDamageable target)
    {
        //GD.Print("attacked,");
        GameLogger.Debug($"{((Node)attacker).Name} attacked {((Node)target).Name} for {_damage} damage", LogCategory.Combat);
        target.TakeDamage(_damage, attacker.GlobalPosition);
    }
}