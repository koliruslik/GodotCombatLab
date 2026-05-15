using CombatLab.Core.FSM;
using Godot;

namespace CombatLab.Core.Managers.RootStateMachine.States;

public partial class MainMenuSceneState : State<RootSM>
{
    private Callable _onPlayClicked;
    public override void Enter()
    {
        base.Enter();
        _onPlayClicked = Callable.From(OnPlayClicked);
        Actor.SceneManager.ChangeScene("res://Scenes/mainMenu.tscn");
        Actor.Connect(RootSM.SignalName.PlayClicked, _onPlayClicked);
    }

    public override void Exit()
    {
        base.Exit();
        Actor.Disconnect(RootSM.SignalName.PlayClicked, _onPlayClicked);
    }
    
    private void OnPlayClicked()
    {
        EmitSignal(SignalName.Transitioned, this, "gameStarted");
    }
}