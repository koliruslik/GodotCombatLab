using CombatLab.Core.Events;
using CombatLab.Core.Utils;
using Godot;

namespace CombatLab.Presentation.UI.HUD;

public partial class InGameSettingsPanel : PanelContainer
{
    [ExportGroup("Buttons")] 
    [Export] public Button BackBtn;

    public override void _Ready()
    {
        if (BackBtn == null) { GameLogger.Error("BackBtn is null"); return; }

        BackBtn.Pressed += EventBus.PublishUIToPauseMenuClicked;
    }

    public override void _ExitTree()
    {
        BackBtn.Pressed -= EventBus.PublishUIToPauseMenuClicked;
    }
}