using System;
using CombatLab.Core.Payloads;
using Godot;

namespace CombatLab.Core.Events;
public static class EventBus
{
    public static event Action<DeathData> OnEnemyDied;
    public static event Action<DeathData> OnPlayerDied;
    
    
    public static event Action<Node, float, float> OnHealthChanged;

    public static event Action<float> OnGoldChanged;
    //public static event Action<int, RollContext ctx> OnD20Rolled; // RollContext not implemented yet

    public static void PublishEnemyDied(DeathData dt) 
        => OnEnemyDied?.Invoke(dt);

    public static void PublishPlayerDeath(DeathData dt)
        => OnPlayerDied?.Invoke(dt);

    public static void PublishHealthChanged(Node source, float currentHP, float maxHP)
        => OnHealthChanged?.Invoke(source, currentHP, maxHP);

    public static void PublishGoldChanged(float currentGold)
        => OnGoldChanged?.Invoke(currentGold);
}

