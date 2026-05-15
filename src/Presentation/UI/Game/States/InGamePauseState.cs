using CombatLab.Core.FSM;
using CombatLab.Presentation.UI.HUD;
using Godot;

namespace CombatLab.Presentation.UI.HUD.States;

[GlobalClass]
public partial class InGamePauseState : State<GameSM>
{
    private Callable _resumeClicked;
    private Callable _settingsClicked;
    private Callable _returnToMenuClicked;
    public override void Enter()
    {
        base.Enter();
        Actor.SetPaused(true);
        Actor.ShowPanel(Actor.PauseMenuPanel);
        _resumeClicked = Callable.From(OnResumeButtonPressed);
        _settingsClicked = Callable.From(OnSettingsButtonPressed);
        _returnToMenuClicked = Callable.From(OnReturnToMenuButtonPressed);
        Actor.Connect(GameSM.SignalName.ResumeClicked,  _resumeClicked);
        Actor.Connect(GameSM.SignalName.SettingsClicked, _settingsClicked);
        Actor.Connect(GameSM.SignalName.ReturnToMainMenu, _returnToMenuClicked);

    }

    public override void Exit()
    {
        base.Exit();
        Actor.SetPaused(false);
        Actor.Disconnect(GameSM.SignalName.ResumeClicked,  _resumeClicked);
        Actor.Disconnect(GameSM.SignalName.SettingsClicked, _settingsClicked);
        Actor.Disconnect(GameSM.SignalName.ReturnToMainMenu, _returnToMenuClicked);
    }

    public override void Update(double delta)
    {
        base.Update(delta);

        if (Input.IsActionJustPressed("ui_cancel"))
        {
            EmitSignal(SignalName.Transitioned, this, "resumeGame");
        }
    }

    private void OnResumeButtonPressed()
    {
        EmitSignal(SignalName.Transitioned, this, "resumeGame");
    }

    private void OnSettingsButtonPressed()
    {
        EmitSignal(SignalName.Transitioned, this, "openSettings");
    }

    private void OnReturnToMenuButtonPressed()
    {
        Actor.RequestedReturnToMenu();
    }
    
}