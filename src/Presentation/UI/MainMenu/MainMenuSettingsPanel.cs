using CombatLab.Core.Events;
using CombatLab.Core.Utils;
using Godot;

namespace CombatLab.Presentation.UI.MainMenu;

public partial class MainMenuSettingsPanel : PanelContainer
{
    [ExportGroup("Buttons")] 
    [Export] public Button BackBtn;

    public override void _Ready()
    {
        if (BackBtn == null) { GameLogger.Error("BackBtn is null"); return; }

        BackBtn.Pressed += EventBus.PublishUIBackClicked;
    }

    public override void _ExitTree()
    {
        BackBtn.Pressed -= EventBus.PublishUIBackClicked;
    }
}