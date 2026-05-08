using Godot;
using System;
using CombatLab.Core.Payloads;
using CombatLab.Core.Events;

namespace CombatLab.Core.Managers;

public partial class GameManager : Node
{
    private int _gold = 0;
    public override void _Ready()
    {
        EventBus.OnEnemyDied += EnemyDiedHandler;
        GD.Print("GameManager Loaded");
    }

    private void EnemyDiedHandler(DeathData dt)
    {
        GD.Print("SlimeDied! +1 Gold");
        _gold++;
        EventBus.PublishGoldChanged(_gold);
    }
}