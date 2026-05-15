using System.Collections.Generic;
using CombatLab.Core.Events;
using Godot;
using CombatLab.Core.Utils;
using Vector2 = Godot.Vector2;

namespace CombatLab.Presentation.UI.MainMenu;

public partial class MainMenuPanel : PanelContainer
{
    [ExportGroup("Buttons")]
    [Export] public Button StartGameBtn;
    [Export] public Button SettingsBtn;
    [Export] public Button ExitGameBtn;
	
    public override void _Ready()
    {
        StartGameBtn.GrabFocus();
        GD.Print("MainMenuPanel Ready, StartGameBtn: " + StartGameBtn?.Name);
        if(StartGameBtn == null) { GameLogger.Error("StartGameBtn not set"); return; }
        if(ExitGameBtn == null) { GameLogger.Error("ExitGameBtn not set"); return; }
        if(SettingsBtn == null) { GameLogger.Error("SettingsBtn not set"); return; }

        StartGameBtn.Pressed  += EventBus.PublishUIStartGameClicked;
        StartGameBtn.Pressed += () => GD.Print("StartGame button pressed!");
        SettingsBtn.Pressed += EventBus.PublishUISettingsClicked;
        ExitGameBtn.Pressed += () => GetTree().Quit();
    }
    
    public override void _ExitTree()
    {
        StartGameBtn.Pressed  -= EventBus.PublishUIStartGameClicked;
        SettingsBtn.Pressed -= EventBus.PublishUISettingsClicked;
    }

}