using Godot;
using System;
using CombatLab.Core.Events;
using CombatLab.Core.Payloads;


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
        GD.Print("SlimeDied! + Gold");
        _gold+= dt.GoldReward;
        EventBus.PublishGoldChanged(_gold);
    }
}