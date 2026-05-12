using Godot;
using System;
using CombatLab.Core.Events;
using CombatLab.Core.Interfaces;
using CombatLab.Core.Payloads;
using CombatLab.Core.Utils;
using CombatLab.Core.Services;
using CombatLab.Presentation.Entities.Player;


namespace CombatLab.Core.Managers;

public partial class GameManager : Node, IGameManager
{
    private int _currentGold;
    public int CurrentGold => _currentGold;

    [Export]
    public LogCategory EnabledLogCategory
    {
        get => GameLogger.EnabledCategories;
        set => GameLogger.EnabledCategories = value;
    }

    public override void _Ready()
    {
        EventBus.EnemyDied += EnemyDiedHandler;
        EventBus.PlayerDied += PlayerDieHandler;
        EventBus.RestartRequested += OnRestartRequested;

        ServiceLocator.Register<IGameManager>(this);
        GameLogger.Info("GameManager Loaded");
    }

    public override void _ExitTree()
    {
        EventBus.EnemyDied -= EnemyDiedHandler;
        EventBus.PlayerDied -= PlayerDieHandler;
        EventBus.RestartRequested -= OnRestartRequested;
        ServiceLocator.Unregister<IGameManager>();
        GameLogger.Info("GameManager Exited");
    }

    private void EnemyDiedHandler(DeathData dt)
    {
        GameLogger.Info($"Enemy died. Gold reward: {dt.GoldReward}. Total: {_currentGold + dt.GoldReward}");
        _currentGold += dt.GoldReward;
        EventBus.PublishGoldChanged(_currentGold);
    }

    private void PlayerDieHandler(DeathData dt)
    {
        GameLogger.Info("Player died. Pausing game.");
        GetTree().Paused = true;
    }

    private void OnRestartRequested()
    {
        GameLogger.Info("Restart requested. Unpausing and reloading scene.");
        GetTree().Paused = false;
        GetTree().ReloadCurrentScene();
    }

}