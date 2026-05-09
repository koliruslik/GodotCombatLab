using CombatLab.Core.Interfaces;

namespace CombatLab.Presentation.Strategies.Attack;

public class MeleeAttack : IAttackStrategy
{
    private float _damage;
    public MeleeAttack(float damage) {_damage = damage;}
    public void Execute(IAttacker attacker, IDamageable target)
    {
        target.TakeDamage(_damage, attacker.GlobalPosition);
    }
}