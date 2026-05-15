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
    [Export] public PackedScene SlimeScene;
    private int _currentGold;
    private int _currentWave = 1;
    private int _slimesAlive = 0;
    public int CurrentGold => _currentGold;

    [Export]
    public LogCategory EnabledLogCategory
    {
        get => GameLogger.EnabledCategories;
        set => GameLogger.EnabledCategories = value;
    }

    public override void _EnterTree()
    {
        ServiceLocator.Register<IGameManager>(this);
    }

    public override void _Ready()
    {
        EventBus.EnemyDied += EnemyDiedHandler;
        EventBus.PlayerDied += PlayerDieHandler;
        //EventBus.RestartRequested += OnRestartRequested; Change to UIRestartClicked Action
        EventBus.PlayerSpawned += OnPlayerSpawned;
        GameLogger.Success("GameManager Ready");
    }

    public override void _ExitTree()
    {
        EventBus.EnemyDied -= EnemyDiedHandler;
        EventBus.PlayerDied -= PlayerDieHandler;
        //EventBus.RestartRequested -= OnRestartRequested; Change to UIRestartClicked Action
        ServiceLocator.Unregister<IGameManager>(); 
        GameLogger.Info("GameManager Exited");
    }

    private void EnemyDiedHandler(DeathData dt)
    {
        GameLogger.Info($"Enemy died. Gold reward: {dt.GoldReward}. Total: {_currentGold + dt.GoldReward}");
        _currentGold += dt.GoldReward;
        _slimesAlive--;
        if (_slimesAlive <= 0)
        {
            _currentWave++; 
            SpawnWave(_currentWave);
        }
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
        ResetStatistics();
        GetTree().ReloadCurrentScene();
    }

    private void OnPlayerSpawned()
    {
        SpawnWave(_currentWave);
    }

    private void SpawnWave(int count)
    {
        _slimesAlive = count;
        for (int i = 0; i < count; i++)
        {
            SpawnSingleSlime();
        }
    }

    private void SpawnSingleSlime()
    {
        Node2D slime = SlimeScene.Instantiate<Node2D>();
        Vector2 spawnPosition = Vector2.Zero; 
        
        Node spawnNode = GetTree().GetFirstNodeInGroup("SpawnPoint");
        if (spawnNode is Node2D spawnPoint2D)
        {
            spawnPosition = spawnPoint2D.GlobalPosition;
        }
        else
        {
            GameLogger.Error("SpawnPoint not found.");
        }
        Vector2 randomOffset = new Vector2(
            (float)GD.RandRange(-40.0f, 40.0f),
            (float)GD.RandRange(-40.0f, 40.0f)
        );
        
        slime.GlobalPosition = spawnPosition + randomOffset;
        
        GetTree().CurrentScene.CallDeferred(Node.MethodName.AddChild, slime);
    }
    
    private void ResetStatistics()
    {
        _currentGold = 0;
        _currentWave = 1;
        _slimesAlive = 0;
    }

}