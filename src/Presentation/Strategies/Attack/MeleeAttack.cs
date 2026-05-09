using CombatLab.Core.Interfaces;

namespace CombatLab.Presentation.Strategies.Attack;

public class MeleeAttack : IAttackStrategy
{
    private int _damage;
    public MeleeAttack(int damage) {_damage = damage;}
    public void Execute(IAttacker attacker, IDamageable target)
    {
        target.TakeDamage(_damage, attacker.GlobalPosition);
    }
}