using System;
using CombatLab.Core.Payloads;
using Godot;

namespace CombatLab.Core.Events;
public static class EventBus
{
    public static event Action<DeathData> EnemyDied;
    public static event Action<DeathData> PlayerDied;
    public static event Action PlayerSpawned;
    public static event Action<Node, float, float> HealthChanged;
    public static event Action<float> GoldChanged;
    //public static event Action<int, RollContext ctx> OnD20Rolled; // RollContext not implemented yet

    public static event Action<GlobalGameState> GameStateChanged;

    public static event Action UIStartGameClicked;
    public static event Action UIReturnToMenuClicked;
    public static event Action UISettingsClicked;
    public static event Action UIResumeClicked;
    public static event Action UIMenuClicked;
    public static event Action UIBackClicked;
    public static event Action UIRestartClicked;
    
    public static void PublishGameStateChanged(GlobalGameState state) => GameStateChanged?.Invoke(state);
    public static void PublishUIStartGameClicked() => UIStartGameClicked?.Invoke();
    public static void PublishUIReturnToMenuClicked() => UIReturnToMenuClicked?.Invoke();
    public static void PublishUISettingsClicked() => UISettingsClicked?.Invoke();
    public static void PublishUIResumeClicked() => UIResumeClicked?.Invoke();
    public static void PublishUIMenuClicked() => UIMenuClicked?.Invoke();
    public static void PublishUIBackClicked() => UIBackClicked?.Invoke();
    public static void PublishUIRestartClicked() => UIRestartClicked?.Invoke();
    
    public static void PublishEnemyDied(DeathData dt) => EnemyDied?.Invoke(dt);
    public static void PublishPlayerDeath(DeathData dt) => PlayerDied?.Invoke(dt);
    public static void PublishHealthChanged(Node source, float currentHP, float maxHP) => HealthChanged?.Invoke(source, currentHP, maxHP);
    public static void PublishGoldChanged(float currentGold) => GoldChanged?.Invoke(currentGold);
    public static void PublishPlayerSpawned() => PlayerSpawned?.Invoke();
    
}

