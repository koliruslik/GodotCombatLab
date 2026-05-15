using CombatLab.Core.FSM;
using CombatLab.Presentation.UI.HUD;
using Godot;

namespace CombatLab.Presentation.UI.HUD.States;

[GlobalClass]
public partial class GameOverState : State<GameSM>
{
    private Callable _onRestartPressed;
    private Callable _onMenuPressed;
    public override void Enter()
    {
        base.Enter();
        Actor.SetPaused(true);
        Actor.ShowPanel(Actor.GameOverPanel);
        _onRestartPressed = Callable.From(OnRestartButtonPressed);
        _onMenuPressed = Callable.From(OnMenuButtonPressed);
        Actor.Connect(GameSM.SignalName.RestartClicked, _onRestartPressed);
        Actor.Connect(GameSM.SignalName.ReturnToMainMenu, _onMenuPressed);
    }

    public override void Exit()
    {
        base.Exit();
        Actor.Disconnect(GameSM.SignalName.RestartClicked, _onRestartPressed);
        Actor.Disconnect(GameSM.SignalName.ReturnToMainMenu, _onMenuPressed);
    }
    
    private void OnRestartButtonPressed()
    {
        EmitSignal(SignalName.Transitioned, this, "restart");
    }

    private void OnMenuButtonPressed()
    {
        Actor.RequestedReturnToMenu();
    }
}