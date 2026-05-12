using System;
using CombatLab.Core.Data.Entities;
using CombatLab.Core.Data.Items.Weapons;
using CombatLab.Core.Events;
using CombatLab.Core.Interfaces;
using CombatLab.Core.Payloads;
using CombatLab.Core.Services;
using CombatLab.Presentation.Components;
using CombatLab.Presentation.Strategies.Attack;
using Godot;
using CombatLab.Core.Utils;

namespace CombatLab.Presentation.Entities.Enemies;

public partial class Enemy : Entity
{
    [Export] public EnemyStats Stats;
    [Export] public HitBox HitBox;
    [Export] public AnimatedSprite2D Sprite;
    public IPlayer Player { get; protected set; }

    protected IAttackStrategy _attackStrategy;
    
    public override void _Ready()
    {
        base._Ready();
        if (Stats == null) { GameLogger.Error($" {Stats.Name}: SlimeStats not set!"); return; }
        if(HitBox == null) { GameLogger.Error($" {Stats.Name}: HitBox not set!"); return; }
        if(Sprite == null) { GameLogger.Error($" {Stats.Name}: Sprite not set!"); return; }
        _attackStrategy = Stats.WeaponData == null
            ? new MeleeAttack(Stats.Damage)
            : Stats.WeaponData.WeaponType.ToStrategy(Stats.WeaponData.Damage);
        
        Health.ZeroHealth += Die;
        GetTree().ProcessFrame += OnFirstFrame;
        HitBox.HitDetected += OnHitDetected;
        EventBus.PlayerDied += OnPlayerDied;
		
        Health.Initialize(Stats.MaxHP, Stats.InvincibleTime);
        AddToGroup("Enemies"); 
    }
    
    public override void _ExitTree()
    {
        Health.ZeroHealth -= Die;
        if(Player != null)
            Player.OnInvincibilityEnded -= HitBox.ResetHits;
        HitBox.HitDetected -= OnHitDetected;
        EventBus.PlayerDied -= OnPlayerDied;
    }

    public void PlayAnimation(string animationName)
    {
        Sprite.Play(animationName);
    }
    
    
    public bool IsPlayerInDetecionRange()
    {
        if(Player == null) return false;
        return GlobalPosition.DistanceTo(Player.GlobalPosition) <= Stats.DetectionRange;
    }

    public bool IsPlayerInAttackRange()
    {
        if(Player == null) return false;
        return  GlobalPosition.DistanceTo(Player.GlobalPosition) <= Stats.AttackRange;
    }

    
    
    protected virtual void OnFirstFrame()
    {
        GetTree().ProcessFrame -= OnFirstFrame;
        if (ServiceLocator.TryGet<IPlayer>(out var player))
        {
            Player = player;
            Player.OnInvincibilityEnded += HitBox.ResetHits;
            GameLogger.Debug($" {Stats.Name}: IPlayer registered in ServiceLocator", LogCategory.Init);
        }
        else
        {
            GameLogger.Error($" {Stats.Name}: Player not found!");
        }
    }
    
    protected virtual void OnPlayerDied(DeathData _)
    {
        GameLogger.Info("Player reference _player set to null");
        Player = null;
    }

    protected virtual void Die()
    {
        var dt = new DeathData
        {
            Victim = this,
            Killer = null,
            DamageSourceId = null,
            Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
            GoldReward = Stats.Gold
        };
        EventBus.PublishEnemyDied(dt);
    }

    protected virtual void OnHitDetected(Node victim)
    {
        GameLogger.Debug($"{Stats.Name} hit: {victim.Name}", LogCategory.Combat);
        if(victim is IDamageable target)
            _attackStrategy.Execute(this, target);
    }
    }