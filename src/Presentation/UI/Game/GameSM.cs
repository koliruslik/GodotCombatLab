using CombatLab.Core.Events;
using CombatLab.Core.FSM;
using CombatLab.Core.Payloads;
using CombatLab.Core.Utils;
using CombatLab.Presentation.UI.HUD.States;
using Godot;

namespace CombatLab.Presentation.UI.HUD;

[GlobalClass]
public partial class GameSM : StateMachine<GameSM>
{
    [Signal] public delegate void ResumeClickedEventHandler();
    [Signal] public delegate void OpenMenuClickedEventHandler();
    [Signal] public delegate void SettingsClickedEventHandler();
    [Signal] public delegate void RestartClickedEventHandler();
    [Signal] public delegate void ReturnToPauseMenuEventHandler();
    [Signal] public delegate void ReturnToMainMenuEventHandler();
    [Signal] public delegate void PlayerDiedEventHandler();
    
    [Export] public PanelContainer GameHUDPanel;
    [Export] public PanelContainer PauseMenuPanel;
    [Export] public PanelContainer InGameSettingsPanel;
    [Export] public PanelContainer GameOverPanel;

    private Control[] _panels;
    
    public bool IsPaused { get; set; }
    protected override void RegisterTransitions()
    {
        _transitions.Add((nameof(InGameState), "pauseMenu"),
            nameof(InGamePauseState));
        _transitions.Add((nameof(InGameState), "playerDied"),
            nameof(GameOverState));
        _transitions.Add((nameof(GameOverState), "restart"),
            nameof(InGameState));
        _transitions.Add((nameof(InGamePauseState), "openSettings"),
            nameof(InGameSettingsState));
        _transitions.Add((nameof(InGamePauseState), "resumeGame"),
            nameof(InGameState));
        _transitions.Add((nameof(InGameSettingsState), "pauseMenu"),
            nameof(InGamePauseState));
    }

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        _panels = new Control[]
        {
            GameHUDPanel,
            PauseMenuPanel, 
            InGameSettingsPanel, 
            GameOverPanel
        };
        EventBus.UIReturnToPauseMenuClicked += OnReturnToPauseMenuClicked;
        EventBus.UIResumeClicked += OnResumeClicked;
        EventBus.UISettingsClicked += OnSettingsClicked;
        EventBus.UIRestartClicked += OnRestartClicked;
        EventBus.PlayerDied += OnPlayerDied;
        SetUp(this);
        
        GameLogger.Success("GameSm Ready");
    }

    public override void _Process(double delta)
    {
        UpdateInput(delta);
    }
    
    public override void _ExitTree()
    {
        GD.Print("GameSM _ExitTree called");
        EventBus.UIReturnToPauseMenuClicked -= OnReturnToPauseMenuClicked;
        EventBus.UIResumeClicked -= OnResumeClicked;
        EventBus.UISettingsClicked -= OnSettingsClicked;
        EventBus.UIRestartClicked -= OnRestartClicked;
        EventBus.PlayerDied -= OnPlayerDied;
    }
    
    public void ShowPanel(Control panel)
    {
        foreach (var p in _panels)
            p.Visible = p == panel;

    }
    
    public void SetPaused(bool paused)
    {
        IsPaused = paused;
        GetTree().Paused = paused;
    }

    public void RequestedReturnToMenu()
    {
        SetPaused(false);
        EventBus.PublishUIReturnToMainMenuClicked();
    }
    
    private void OnResumeClicked()
        => EmitSignal(SignalName.ResumeClicked);

    private void OnSettingsClicked()
        => EmitSignal(SignalName.SettingsClicked);

    private void OnReturnToPauseMenuClicked()
        => EmitSignal(SignalName.ReturnToPauseMenu);

    private void OnReturnToMainMenuClicked()
        => EmitSignal(SignalName.ReturnToMainMenu);

    private void OnRestartClicked()
        => EmitSignal(SignalName.RestartClicked);

    private void OnPlayerDied(DeathData _)
        => EmitSignal(SignalName.PlayerDied);
}