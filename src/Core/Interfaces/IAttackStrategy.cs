using CombatLab.Presentation.Entities;

namespace CombatLab.Core.Interfaces;

public interface IAttackStrategy
{
    void Execute(IAttacker attacker, IDamageable target);
}