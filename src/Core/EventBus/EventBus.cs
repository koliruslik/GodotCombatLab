using System;
using CombatLab.Core.Payloads;
using Godot;

namespace CombatLab.Core.Events;
public static class EventBus
{
    public static event Action RestartRequested;
    public static event Action<DeathData> EnemyDied;
    public static event Action<DeathData> PlayerDied;

    public static event Action PlayerSpawned;
    
    public static event Action<Node, float, float> HealthChanged;

    public static event Action<float> GoldChanged;
    //public static event Action<int, RollContext ctx> OnD20Rolled; // RollContext not implemented yet

    public static void PublishRestartRequested()
        => RestartRequested?.Invoke();
    public static void PublishEnemyDied(DeathData dt) 
        => EnemyDied?.Invoke(dt);

    public static void PublishPlayerDeath(DeathData dt)
        => PlayerDied?.Invoke(dt);
    
    public static void PublishHealthChanged(Node source, float currentHP, float maxHP)
        => HealthChanged?.Invoke(source, currentHP, maxHP);

    public static void PublishGoldChanged(float currentGold)
        => GoldChanged?.Invoke(currentGold);
    
}

