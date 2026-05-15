using CombatLab.Core.Events;
using CombatLab.Core.FSM;
using CombatLab.Core.Utils;
using CombatLab.Presentation.UI.MainMenu.States;
using Godot;

namespace CombatLab.Presentation.UI.MainMenu;

[GlobalClass]
public partial class MainMenuSM : StateMachine<MainMenuSM>
{
    [Signal] public delegate void SettingsClickedEventHandler();
    [Signal] public delegate void ReturnToMenuClickedEventHandler();
    [Signal] public delegate void ExitClickedEventHandler();
    
    [Export] public Control MainMenuPanel;
    [Export] public Control SettingsPanel;

    private Control[] _panels;
    protected override void RegisterTransitions()
    {
        _transitions.Add((nameof(MainMenuState), "openSettings"), nameof(MainMenuSettingsState));
        _transitions.Add((nameof(MainMenuSettingsState), "backToMenu"), nameof(MainMenuState));
    }

    public override void _Ready()
    {
        EventBus.UISettingsClicked += OnSettingsClicked;
        EventBus.UIMainMenuBackClicked += OnBackClicked;
        
        _panels = new Control[]
        {
            MainMenuPanel,
            SettingsPanel
        };
        
        if (MainMenuPanel == null || SettingsPanel == null)
        {
            GameLogger.Error("Panels not set in MainMenuSM");
            return;
        }
        
        SetUp(this);
        GameLogger.Success("MainMenuSM ready");
    }

    public override void _ExitTree()
    {
        EventBus.UISettingsClicked -= OnSettingsClicked;
        EventBus.UIMainMenuBackClicked -= OnBackClicked;
    }

    public void ShowPanel(Control panel)
    {
        foreach (var p in _panels)
            p.Visible = p == panel;

    }

    private void OnSettingsClicked()
        => EmitSignal(SignalName.SettingsClicked);

    private void OnBackClicked()
        => EmitSignal(SignalName.ReturnToMenuClicked);

}