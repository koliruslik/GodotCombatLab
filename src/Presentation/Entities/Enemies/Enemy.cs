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
    [Export] AnimatedSprite2D Sprite;

    protected IAttackStrategy _attackStrategy;

    protected IPlayer _player;
    

    protected string _currentAnimation = "";

    protected const string DIE_ANIMATION = "die";
    
    public override void _Ready()
    {
        base._Ready();
        if (Stats == null) { GameLogger.Error($" {Stats.Name}: SlimeStats not set!"); return; }
        if(HitBox == null) { GameLogger.Error($" {Stats.Name}: HitBox not set!"); return; }
        if(Sprite == null) { GameLogger.Error($" {Stats.Name}: Sprite not set!"); return; }
        _attackStrategy = Stats.WeaponData == null
            ? new MeleeAttack(Stats.Damage)
            : Stats.WeaponData.WeaponType.ToStrategy(Stats.WeaponData.Damage);
		
        Sprite.AnimationFinished += OnAnimationFinished;
        Health.DamageTaken += OnDamageTaken;
        Health.ZeroHealth += Die;
        GetTree().ProcessFrame += OnFirstFrame;
        HitBox.HitDetected += OnHitDetected;
        EventBus.PlayerDied += OnPlayerDied;
		
        Health.Initialize(Stats.MaxHP);
        AddToGroup("Enemies"); 
    }
    
    public override void _ExitTree()
    {
        Sprite.AnimationFinished -= OnAnimationFinished;
        Health.DamageTaken -= OnDamageTaken;
        Health.ZeroHealth -= Die;
        if(_player != null)
            _player.OnInvincibilityEnded -= HitBox.ResetHits;
        HitBox.HitDetected -= OnHitDetected;
        EventBus.PlayerDied -= OnPlayerDied;
    }
    
    protected virtual void OnFirstFrame()
    {
        GetTree().ProcessFrame -= OnFirstFrame;
        if (ServiceLocator.TryGet<IPlayer>(out var player))
        {
            _player = player;
            _player.OnInvincibilityEnded += HitBox.ResetHits;
            GameLogger.Debug($" {Stats.Name}: IPlayer registered in ServiceLocator", LogCategory.Init);
        }
        else
        {
            GameLogger.Error($" {Stats.Name}: Player not found!");
        }
    }
    
    protected virtual void OnDamageTaken(Vector2 sourcePosition)
    {
        if (_player == null) return;
        GameLogger.Debug($"{Stats.Name}: took damage.", LogCategory.Combat);
        Sprite.Modulate = Colors.Red;
        //_isHurting = true; // Change with state machine
        //Velocity = knockBackDir * 200;
        ResetHurtState();
    }

    protected virtual void OnPlayerDied(DeathData _)
    {
        GameLogger.Info("Player reference _player set to null");
        _player = null;
    }
    
    protected virtual void Die()
    {
        var dt = new DeathData
        {
            Victim = this,
            Killer = null,
            DamageSourceId = null,
            Timestamp =  DateTimeOffset.Now.ToUnixTimeMilliseconds(),
            GoldReward = Stats.Gold
        };
        EventBus.PublishEnemyDied(dt);
        _currentAnimation = DIE_ANIMATION;
        Sprite.Play(_currentAnimation);
    }
    
    protected virtual void OnAnimationFinished()
    {
        if (_currentAnimation == DIE_ANIMATION)
        {
            QueueFree();
        }
    }
	
    protected virtual void OnHitDetected(Node victim)
    {
        GameLogger.Debug($"{Stats.Name} hit: {victim.Name}", LogCategory.Combat);
        if(victim is IDamageable target)
            _attackStrategy.Execute(this, target);
    }
    
    private void ResetHurtState()
    {
        GetTree().CreateTimer(0.2f).Timeout += () =>
        {
            if (!IsInstanceValid(this)) return;
            Sprite.Modulate = Colors.White;
            //_isHurting = false;
        };
    }
}