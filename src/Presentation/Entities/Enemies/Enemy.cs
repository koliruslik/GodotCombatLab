using System;
using CombatLab.Core.Data.Entities;
using CombatLab.Core.Data.Items.Weapons;
using CombatLab.Core.Events;
using CombatLab.Core.Interfaces;
using CombatLab.Core.Payloads;
using CombatLab.Presentation.Components;
using CombatLab.Presentation.Strategies.Attack;
using Godot;
using GodotCombatLab.Core.Utils;

namespace CombatLab.Presentation.Entities.Enemies;

public partial class Enemy : Entity
{
    [Export] public EnemyStats Stats;
    [Export] public HitBox HitBox;
    [Export] AnimatedSprite2D Sprite;

    protected IAttackStrategy _attackStrategy;

    protected Node _player;
    

    protected string _currentAnimation = "";

    protected const string DIE_ANIMATION = "die";
    
    public override void _Ready()
    {
        base._Ready();
        if (Stats == null) { GameLogger.Error("SlimeStats not set!"); return; }
        if(HitBox == null) { GameLogger.Error("HitBox not set!"); return; }
        if(Sprite == null) { GameLogger.Error("Sprite not set!"); return; }
        _attackStrategy = Stats.WeaponData == null
            ? new MeleeAttack(Stats.Damage)
            : Stats.WeaponData.WeaponType.ToStrategy(Stats.WeaponData.Damage);
		
        Sprite.AnimationFinished += OnAnimationFinished;
        Health.DamageTaken += OnDamageTaken;
        Health.ZeroHealth += Die;
        GetTree().ProcessFrame += OnFirstFrame;
        HitBox.HitDetected += OnHitDetected;
		
        Health.Initialize(Stats.MaxHP);
        AddToGroup("Enemies"); 
    }
    
    public override void _ExitTree()
    {
        Sprite.AnimationFinished -= OnAnimationFinished;
        Health.DamageTaken -= OnDamageTaken;
        Health.ZeroHealth -= Die;
        if(_player is Entity entity)
            entity.Health.InvincibilityEnded -= HitBox.ResetHits;
        HitBox.HitDetected -= OnHitDetected;
    }
    
    protected virtual void OnFirstFrame()
    {
        GetTree().ProcessFrame -= OnFirstFrame;
        _player = GetTree().GetFirstNodeInGroup("Player");
        GameLogger.Debug($"Player found: {_player?.Name}, is Entity: {_player is Entity}");
        if(_player is Entity entity)
        {
            GameLogger.Debug("Subscribed to InvincibilityEnded!");
            entity.Health.InvincibilityEnded += HitBox.ResetHits;
        }
    }
    
    protected virtual void OnDamageTaken(Vector2 sourcePosition)
    {
        GameLogger.Debug($"Enemy {Stats.Name} took damage.");
        Sprite.Modulate = Colors.Red;
        //_isHurting = true; // Change with state machine
        //Velocity = knockBackDir * 200;
        ResetHurtState();
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
        if (_currentAnimation == "die")
        {
            QueueFree();
        }
    }
	
    protected virtual void OnHitDetected(Node victim)
    {
        GameLogger.Debug($"{Stats.Name} hit: {victim.Name}");
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