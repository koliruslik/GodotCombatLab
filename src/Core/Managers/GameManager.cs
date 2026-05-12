using Godot;
using System;
using CombatLab.Core.Events;
using CombatLab.Core.Interfaces;
using CombatLab.Core.Payloads;
using CombatLab.Core.Utils;
using CombatLab.Core.Services;



namespace CombatLab.Core.Managers;

public partial class GameManager : Node, IGameManager
{
    private int _currentGold;
    public int CurrentGold => _currentGold;
    public override void _Ready()
    {
        GameLogger.EnabledCategories = LogCategory.State | LogCategory.Init;
        
        EventBus.EnemyDied += EnemyDiedHandler;
        
        ServiceLocator.Register<IGameManager>(this);
        GameLogger.Info("GameManager Loaded");
    }

    public override void _ExitTree()
    {
        EventBus.EnemyDied -= EnemyDiedHandler;
        ServiceLocator.Unregister<IGameManager>();
        GameLogger.Info("GameManager Exited");
    }

    private void EnemyDiedHandler(DeathData dt)
    {
        GameLogger.Info("SlimeDied! + Gold");
        _currentGold+= dt.GoldReward;
        EventBus.PublishGoldChanged(_currentGold);
    }
}