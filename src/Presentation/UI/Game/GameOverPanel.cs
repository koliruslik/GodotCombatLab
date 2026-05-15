using CombatLab.Core.Events;
using CombatLab.Core.Utils;
using Godot;

namespace CombatLab.Presentation.UI.HUD;

public partial class GameOverPanel : PanelContainer
{
    [ExportGroup("Buttons")]
    [Export] public Button RestartBtn;
    [Export] public Button ToMenuBtn;
	
    public override void _Ready()
    {
        if(RestartBtn == null) { GameLogger.Error("RestartBtn not set"); return; }
        if(ToMenuBtn == null) { GameLogger.Error("ToMenuBtn not set"); return; }

        RestartBtn.Pressed  += EventBus.PublishUIRestartClicked;
        ToMenuBtn.Pressed += EventBus.PublishUIReturnToMainMenuClicked;
    }
    
    public override void _ExitTree()
    {
        RestartBtn.Pressed  -= EventBus.PublishUIRestartClicked;
        ToMenuBtn.Pressed -= EventBus.PublishUIReturnToMainMenuClicked;
    }
}