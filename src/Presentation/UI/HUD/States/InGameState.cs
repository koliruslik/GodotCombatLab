using CombatLab.Core.FSM;
using CombatLab.Presentation.UI.HUD;
using Godot;

namespace CombatLab.Presentation.UI.HUD.States;

[GlobalClass]
public partial class InGameState : State<HUD>
{
    public override void Enter()
    {
        base.Enter();
        GetTree().Paused = false;
    }

    public override void Update(double delta)
    {
        base.Update(delta);

        if (Input.IsActionJustPressed("ui_cancel"))
        {
            EmitSignal(SignalName.Transitioned, this, "pauseMenu");
        }

    }
    
    private void OnPlayerDied()
    {
        EmitSignal(SignalName.Transitioned, this, "gameOver");
    }
}