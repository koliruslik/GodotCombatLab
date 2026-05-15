using System;
using CombatLab.Core.Events;
using Godot;
using CombatLab.Core.FSM;
using CombatLab.Core.Interfaces;
using CombatLab.Core.Managers.RootStateMachine.States;
using CombatLab.Core.Services;
using CombatLab.Core.Utils;

namespace CombatLab.Core.Managers.RootStateMachine;

[GlobalClass]
public partial class RootSM : StateMachine<RootSM>
{ 
    [Signal] public delegate void PlayClickedEventHandler();
    [Signal] public delegate void ReturnToMenuClickedEventHandler();
    
    public ISceneManager SceneManager { get; private set; }
    protected override void RegisterTransitions()
    {
        _transitions.Add((nameof(MainMenuSceneState), "gameStarted"), nameof(GameSceneState));
        _transitions.Add((nameof(GameSceneState), "returnToMainMenu"), nameof(MainMenuSceneState));
    }

    public override void _Ready()
    {
        EventBus.UIStartGameClicked += OnUIPlayClicked;
        EventBus.UIReturnToMenuClicked += OnReturnToMenuClicked;
        if (!ServiceLocator.TryGet<ISceneManager>(out var sceneManager))
        {
            GameLogger.Error("ISceneManager not registered");
            return;
        }
        SceneManager = sceneManager;
        
        SetUp(this);
    }

    public override void _ExitTree()
    {
        EventBus.UIStartGameClicked -= OnUIPlayClicked;
        EventBus.UIReturnToMenuClicked -= OnReturnToMenuClicked;
    }
    
    private void OnUIPlayClicked()
    {
        EmitSignal(SignalName.PlayClicked);
    }

    private void OnReturnToMenuClicked()
    {
        EmitSignal(SignalName.ReturnToMenuClicked);
    }
    
}