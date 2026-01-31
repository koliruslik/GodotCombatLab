using Godot;

namespace CombatLab.entities.components;

[GlobalClass]
public partial class HurtBox : Area2D
{
    [Export] public Node OwnerNode;

    public void ReceiveDamage(int amount)
    {
        if (OwnerNode is IDamageable target)
        {
            target.TakeDamage(amount);
        }
    }
}