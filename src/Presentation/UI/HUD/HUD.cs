using CombatLab.Core.Events;
using CombatLab.Core.Payloads;
using CombatLab.Core.Utils;
using CombatLab.Presentation.Entities.Player;
using Godot;

namespace CombatLab.Presentation.UI.HUD;

public partial class HUD : Node
{
    [Export] Label HealthLabel;
    [Export] Label GoldLabel;
    [Export] public Control DeathScreen;
    [Export] public Button RestartButton;
    
    public override void _Ready()
    {
        if (HealthLabel == null) { GameLogger.Error($"{HealthLabel.Name} is not set!"); return; }
        if (GoldLabel == null) { GameLogger.Error($"{GoldLabel.Name} is not set!"); return; }
        if (DeathScreen == null) { GameLogger.Error($"{DeathScreen.Name} is not set!"); return; }
        if (RestartButton == null) { GameLogger.Error($"{RestartButton.Name} is not set!"); return; }
        
        EventBus.GoldChanged += GoldChangedHandler;
        EventBus.HealthChanged += HealthChangedHandler;
        EventBus.PlayerDied += PlayerDieHandler;
        RestartButton.Pressed += OnRestartPressed;
        DeathScreen.Visible = false;
        GoldLabel.Text = "Gold: 0"; 
    }

    public override void _ExitTree()
    {
        EventBus.GoldChanged -= GoldChangedHandler;
        EventBus.HealthChanged -= HealthChangedHandler;
        EventBus.PlayerDied -= PlayerDieHandler;
        RestartButton.Pressed -= OnRestartPressed;
    }
    
    private void PlayerDieHandler(DeathData dt)
    {
        GameLogger.Info("HUD: Player died. Showing death screen.");
        DeathScreen.Visible = true;
    }

    private void OnRestartPressed()
    {
        GameLogger.Info("HUD: Restart button pressed."); 
        EventBus.PublishUIRestartClicked();
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