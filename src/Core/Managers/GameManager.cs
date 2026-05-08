using Godot;
using System;
using CombatLab.Core.Payloads;
using CombatLab.Core.Events;

namespace CombatLab.Core.Managers;

public partial class GameManager : Node
{
    public override void _Ready()
    {
        EventBus.OnEnemyDied += EnemyDieHandler;
        GD.Print("GameManager Loaded");
    }

    private void EnemyDieHandler(DeathData dt)
    {
        GD.Print("SlimeDied!");
    }
}