using CombatLab.Core.FSM;
using CombatLab.Presentation.UI.MainMenu;
using Godot;

namespace CombatLab.Presentation.UI.MainMenu.States;

[GlobalClass]
public partial class MainMenuSettingsState : State<MainMenu>
{
    public override void Enter()
    {
        base.Enter();
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void OnBackButtonPressed()
    {
        if (GetTree().Paused)
        {
            EmitSignal(SignalName.Transitioned, this, "backToPause");
        }
        else
        {
            EmitSignal(SignalName.Transitioned, this, "backToMenu");
        }
    }
}