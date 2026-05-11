using CombatLab.Core.Events;
using CombatLab.Presentation.Entities.Player;
using Godot;

namespace CombatLab.Presentation.UI.HUD;

public partial class HUD : Node
{
    [Export] Label HealthLabel;
    [Export] Label GoldLabel;
    
    private Node _player;
    public override void _Ready()
    {
        EventBus.GoldChanged += GoldChangedHandler;
        EventBus.HealthChanged += HealthChangedHandler;
        _player = GetTree().GetFirstNodeInGroup("Player");
        GoldLabel.Text = "Gold: 0"; 
    }

    public override void _ExitTree()
    {
        EventBus.GoldChanged -= GoldChangedHandler;
        EventBus.HealthChanged -= HealthChangedHandler;
    }
    
    private void GoldChangedHandler(float currentGold)
    {
        GoldLabel.Text = $"Gold: {currentGold}";
    }

    private void HealthChangedHandler(Node source, float currentHP, float maxHP)
    {
        if (source is not Player) return;
        HealthLabel.Text = $"Health: {currentHP}/{maxHP}";
    }
}