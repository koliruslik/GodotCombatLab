using CombatLab.Core.FSM;
using CombatLab.Presentation.UI.HUD;
using Godot;

namespace CombatLab.Presentation.UI.HUD.States;

[GlobalClass]
public partial class GameOverState : State<HUD>
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
    
    private void OnRestartButtonPressed()
    {
        EmitSignal(SignalName.Transitioned, this, "restartGame");
    }

    private void OnMenuButtonPressed()
    {
        EmitSignal(SignalName.Transitioned, this, "exitToMenu");
    }
}