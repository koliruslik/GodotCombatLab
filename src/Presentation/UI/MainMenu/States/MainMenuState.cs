using CombatLab.Core.FSM;
using CombatLab.Presentation.UI.MainMenu;
using Godot;

namespace CombatLab.Presentation.UI.MainMenu.States;

[GlobalClass]
public partial class MainMenuState : State<MainMenuSM>
{
    
    private Callable _onSettingsPressed;
    
    public override void Enter()
    {
        base.Enter();
        Actor.ShowPanel(Actor.MainMenuPanel);
        _onSettingsPressed = Callable.From(OnSettingsPressed);
        Actor.Connect(MainMenuSM.SignalName.SettingsClicked, _onSettingsPressed);
    }

    public override void Exit()
    {
        base.Exit();
        Actor.Disconnect(MainMenuSM.SignalName.SettingsClicked, _onSettingsPressed);
    }
    
    private void OnSettingsPressed()
    {
        EmitSignal(SignalName.Transitioned, this, "openSettings");
    }
    
}