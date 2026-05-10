using CombatLab.Core.Interfaces;
using Godot;

namespace CombatLab.Presentation.Strategies.Attack;

public class RangedAttack : IAttackStrategy
{
    private float _damage;
    
    public RangedAttack(float damage) {_damage = damage;}
    public void Execute(IAttacker attacker, IDamageable target)
    {
        GD.PushWarning("Not Implemented Yet");
    }
}