using CombatLab.Core.Events;
using CombatLab.Core.Payloads;
using CombatLab.Core.Utils;
using CombatLab.Presentation.Entities.Player;
using Godot;

namespace CombatLab.Presentation.UI.HUD;

public partial class HUD : PanelContainer
{
    [Export] Label HealthLabel;
    [Export] Label GoldLabel;
    
    public override void _Ready()
    {
        if (HealthLabel == null) { GameLogger.Error($"{HealthLabel.Name} is not set!"); return; }
        if (GoldLabel == null) { GameLogger.Error($"{GoldLabel.Name} is not set!"); return; }
       
        EventBus.GoldChanged += GoldChangedHandler;
        EventBus.HealthChanged += HealthChangedHandler;
        GoldLabel.Text = "Gold: 0"; 
    }

    public override void _ExitTree()
    {
        EventBus.GoldChanged -= GoldChangedHandler;
        EventBus.HealthChanged -= HealthChangedHandler;
    }
    private void GoldChangedHandler(float currentGold)
    {
        GameLogger.Debug($"HUD: Gold updated: {currentGold}", LogCategory.UI);
        GoldLabel.Text = $"Gold: {currentGold}";
    }
    
    private void HealthChangedHandler(Node source, float currentHP, float maxHP)
    {
        if (source is not Player) return;
        GameLogger.Debug($"HUD: Health updated: {currentHP}/{maxHP}", LogCategory.UI);
        HealthLabel.Text = $"Health: {currentHP}/{maxHP}";
    }
}