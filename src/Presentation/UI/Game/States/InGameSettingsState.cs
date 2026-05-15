using CombatLab.Core.FSM;
using CombatLab.Presentation.UI.HUD;
using Godot;

namespace CombatLab.Presentation.UI.HUD.States;

[GlobalClass]
public partial class InGameSettingsState : State<GameSM>
{
    private Callable _onReturnToPauseMenuClicked;
    public override void Enter()
    {
        base.Enter();
        Actor.SetPaused(true);
        Actor.ShowPanel(Actor.InGameSettingsPanel);

        _onReturnToPauseMenuClicked = Callable.From(OnBackClicked);
        Actor.Connect(GameSM.SignalName.ReturnToPauseMenu, _onReturnToPauseMenuClicked);
    }

    public override void Exit()
    {
        base.Exit();
        Actor.Disconnect(GameSM.SignalName.ReturnToPauseMenu, _onReturnToPauseMenuClicked);
    }

    public override void Update(double delta)
    {
        base.Update(delta);

        if (Input.IsActionJustPressed("ui_cancel"))
        {
            EmitSignal(SignalName.Transitioned, this, "pauseMenu");
        }
    }
    
    private void OnBackClicked()
    {
        EmitSignal(SignalName.Transitioned, this, "pauseMenu");
    }
}