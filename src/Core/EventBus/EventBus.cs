using System;
using CombatLab.Core.Payloads;

namespace CombatLab.Core.Events;
public static class EventBus
{
    public static event Action<DeathData> OnEnemyDied;
    public static event Action<DeathData> OnPlayerDied;
    //public static event Action<int, RollContext ctx> OnD20Rolled; // RollContext not implemented yet

    public static void PublishEnemyDied(DeathData dt)
    {
        OnEnemyDied?.Invoke(dt);
    }
}
