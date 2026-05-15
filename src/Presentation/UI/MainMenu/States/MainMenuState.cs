using CombatLab.Core.FSM;
using CombatLab.Presentation.UI.MainMenu;
using Godot;

namespace CombatLab.Presentation.UI.MainMenu.States;

[GlobalClass]
public partial class MainMenuState : State<MainMenu>
{
    public override void Enter()
    {
        base.Enter();

        GetTree().Paused = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void OnPlayButtonPressed()
    {
        EmitSignal(SignalName.Transitioned, this, "startGame");
    }

    private void OnSettingsButtonPressed()
    {
        EmitSignal(SignalName.Transitioned, this, "openSettings");
    }
    
}