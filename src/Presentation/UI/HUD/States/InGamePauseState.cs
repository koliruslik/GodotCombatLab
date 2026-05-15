using CombatLab.Core.FSM;
using CombatLab.Presentation.UI.HUD;
using Godot;

namespace CombatLab.Presentation.UI.HUD.States;

[GlobalClass]
public partial class InGamePauseState : State<HUD>
{
    public override void Enter()
    {
        base.Enter();
        GetTree().Paused = true;
    }

    public override void Exit()
    {
        base.Exit();
        GetTree().Paused = false;
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

    private void OnQuitButtonPressed()
    {
        EmitSignal(SignalName.Transitioned, this, "quitGame");
    }
    
}