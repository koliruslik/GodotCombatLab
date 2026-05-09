using Godot;

namespace CombatLab.Core.Data;

[GlobalClass]
public partial class EnemyStats : Resource
{
    [Export] public float MaxHP;
    [Export] public float MaxMP;
    [Export] public float Atack;
    [Export] public float Defense;
    [Export] public float Speed;
    [Export] public float JumpPower;
    [Export] public int Gold;
    //[Export] public Strategy AttackStrategy; // not implemented
}