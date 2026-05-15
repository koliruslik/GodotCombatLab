using CombatLab.Core.Events;
using CombatLab.Core.Utils;
using Godot;

namespace CombatLab.Presentation.UI.HUD;

public partial class PauseMenuPanel : PanelContainer
{
    [ExportGroup("Buttons")]
    [Export] public Button ResumeGameBtn;
    [Export] public Button SettingsBtn;
    [Export] public Button ToMenuBtn;
	
    public override void _Ready()
    {
        if(ResumeGameBtn == null) { GameLogger.Error("ResumeGameBtn not set"); return; }
        if(ToMenuBtn == null) { GameLogger.Error("ToMenuBtn not set"); return; }
        if(SettingsBtn == null) { GameLogger.Error("SettingsBtn not set"); return; }

        ResumeGameBtn.Pressed  += EventBus.PublishUIResumeClicked;
        SettingsBtn.Pressed += EventBus.PublishUISettingsClicked;
        ToMenuBtn.Pressed += EventBus.PublishUIReturnToMainMenuClicked;
    }
    
    public override void _ExitTree()
    {
        ResumeGameBtn.Pressed  -= EventBus.PublishUIResumeClicked;
        SettingsBtn.Pressed -= EventBus.PublishUISettingsClicked;
        ToMenuBtn.Pressed -= EventBus.PublishUIReturnToMainMenuClicked;
    }
    
}