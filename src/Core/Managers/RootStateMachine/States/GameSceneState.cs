using CombatLab.Core.FSM;
using Godot;

namespace CombatLab.Core.Managers.RootStateMachine.States;

[GlobalClass]
public partial class GameSceneState : State<RootSM>
{
    private Callable _onReturnClicked;
    public override void Enter()
    {
        base.Enter();
        _onReturnClicked = Callable.From(OnReturnClicked);
        Actor.SceneManager.ChangeScene("res://scenes/levels/level_1.tscn");
        Actor.Connect(RootSM.SignalName.ReturnToMenuClicked, _onReturnClicked);
    }

    public override void Exit()
    {
        base.Exit();
        Actor.Disconnect(RootSM.SignalName.ReturnToMenuClicked, _onReturnClicked);
    }

    private void OnReturnClicked()
    {
        EmitSignal(SignalName.Transitioned, this, "returnToMainMenu");
    }
}