
using CombatLab.Core.Events;
using Godot;

namespace CombatLab.Presentation.UI.HUD;

public partial class HUD : Node
{
    [Export] Label HealthLabel;
    [Export] Label GoldLabel;
    public override void _Ready()
    {
        EventBus.OnGoldChanged += GoldChangedHandler;
        EventBus.OnHealthChanged += HealthChangedHandler;
        HealthLabel.Text = "Health: 100/100";
        GoldLabel.Text = "Gold: 0";
    }

    public override void _ExitTree()
    {
        EventBus.OnGoldChanged -= GoldChangedHandler;
        EventBus.OnHealthChanged -= HealthChangedHandler;
    }

    private void GoldChangedHandler(float currentGold)
    {
        GoldLabel.Text = $"Gold: {currentGold}";
    }

    private void HealthChangedHandler(float currentHP, float maxHP)
    {
        HealthLabel.Text = $"Health: {currentHP}/{maxHP}";
    }
}