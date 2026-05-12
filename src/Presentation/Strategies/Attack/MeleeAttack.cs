using CombatLab.Core.Interfaces;
using Godot;
using CombatLab.Core.Utils;

namespace CombatLab.Presentation.Strategies.Attack;

public class MeleeAttack : IAttackStrategy
{
    private float _damage;
    private float _knockbackForce;
    private float _knockbackLift;

    public MeleeAttack(float damage, float knockbackForce, float knockbackLift)
    {
        _damage = damage;
        _knockbackForce = knockbackForce;
        _knockbackLift = knockbackLift;
    }

    public MeleeAttack(float damage)
    {
        _damage = damage;
        _knockbackForce = 0f;
    }
    public void Execute(IAttacker attacker, IDamageable target)
    {
        //GD.Print("attacked,");
        GameLogger.Debug($"{((Node)attacker).Name} attacked {((Node)target).Name} for {_damage} damage", LogCategory.Combat);
        if (target is IKnockbackable kb)
        {
            kb.ApplyKnockback(attacker.GlobalPosition, _knockbackForce, _knockbackLift);
            GameLogger.Debug($"Target is IKnockbackable: {target is IKnockbackable}, Force: {_knockbackForce}", LogCategory.Combat);
        }
        target.TakeDamage(_damage, attacker.GlobalPosition);
    }
}