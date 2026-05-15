using CombatLab.Core.FSM;
using CombatLab.Presentation.UI.MainMenu;
using Godot;

namespace CombatLab.Presentation.UI.MainMenu.States;

[GlobalClass]
public partial class MainMenuSettingsState : State<MainMenuSM>
{
    private Callable _onBackPressed;
    public override void Enter()
    {
        base.Enter();
        Actor.ShowPanel(Actor.SettingsPanel);
        _onBackPressed = Callable.From(OnBackPressed);
        Actor.Connect(MainMenuSM.SignalName.ReturnToMenuClicked, _onBackPressed);
    }

    public override void Exit()
    {
        base.Exit();
        Actor.Disconnect(MainMenuSM.SignalName.ReturnToMenuClicked, _onBackPressed);
    }

    private void OnBackPressed()
    {
        EmitSignal(SignalName.Transitioned, this, "backToMenu");
    }

}