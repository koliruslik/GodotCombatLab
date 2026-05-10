using CombatLab.Core.Interfaces;
using Godot;

namespace CombatLab.Presentation.Strategies.Attack;

public class MeleeAttack : IAttackStrategy
{
    private float _damage;
    public MeleeAttack(float damage) {_damage = damage;}
    public void Execute(IAttacker attacker, IDamageable target)
    {
        //GD.Print("attacked,");
        GD.Print($"{((Node)attacker).Name} attacked {((Node)target).Name} for {_damage} damage");
        target.TakeDamage(_damage, attacker.GlobalPosition);
    }
}