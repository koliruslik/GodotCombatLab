using CombatLab.entities.player;
using Godot;

namespace CombatLab.entities.components;

[GlobalClass]
public partial class HitBox : Area2D
{
    [Export] public int Damage = 10;

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is HurtBox hurtBox)
        {
            hurtBox.ReceiveDamage(Damage);
        }
    }
}