using CombatLab.Core.Interfaces;
using Godot;

namespace CombatLab.Core.Data.Entities;

[GlobalClass]
public partial class EnemyStats : Resource
{
    [Export] public float MaxHP;
    [Export] public float MaxMP;
    [Export] public float Attack;
    [Export] public float Defense;
    [Export] public float Speed;
    [Export] public float JumpVelocity;
    [Export] public int Gold;
    //[Export] public IAttackStrategy AttackStrategy; 
}