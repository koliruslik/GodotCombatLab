using CombatLab.Core.FSM;
using CombatLab.Presentation.UI.HUD;
using Godot;

namespace CombatLab.Presentation.UI.HUD.States;

[GlobalClass]
public partial class InGameState : State<GameSM>
{
    private Callable _onPlayerDied;
    public override void Enter()
    {
        base.Enter();
        Actor.SetPaused(false);
        Actor.ShowPanel(Actor.GameHUDPanel);

        _onPlayerDied = Callable.From(OnPlayerDied);
        Actor.Connect(GameSM.SignalName.PlayerDied, _onPlayerDied);
    }

    public override void Exit()
    {
        base.Exit();
        Actor.Disconnect(GameSM.SignalName.PlayerDied, _onPlayerDied);
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
        EmitSignal(SignalName.Transitioned, this, "playerDied");
    }
}