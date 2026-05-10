using CombatLab.Core.Interfaces;
using Godot;

namespace CombatLab.Presentation.Strategies.Attack;

public class MageAttack : IAttackStrategy
{
    private float _damage;
    public MageAttack(float damage) {_damage = damage;}
    public void Execute(IAttacker attacker, IDamageable target)
    {
        GD.PushWarning("Not Implemented Yet");
    }
}