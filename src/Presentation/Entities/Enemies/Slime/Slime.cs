using Godot;
using System;
using CombatLab.Core.Data.Entities;
using CombatLab.Core.Events;
using CombatLab.Core.Interfaces;
using CombatLab.Core.Payloads;
using CombatLab.Presentation.Components;

namespace CombatLab.Presentation.Entities.Enemies;

public partial class Slime : Entity
{
	[Export] public EnemyStats SlimeStats;
	[Export] public HitBox HitBox;
	[Export] public HurtBox HurtBox;
	private float _currentHP;
	
	private Node2D _player;
	
	private AnimatedSprite2D _sprite;
	private bool _isHurting = false;
	
	private string _currentAnimation = "";

	public override void _Ready()
	{
		base._Ready();
		if (SlimeStats == null) { GD.PushError("SlimeStats not set!"); return; }
		if(HitBox == null) { GD.PushError("HitBox not set!"); return; }
		if(HurtBox == null) { GD.PushError("HurtBox not set!"); return; }
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
		
		
		_sprite.AnimationFinished += OnAnimationFinished;
		Health.DamageTaken += OnDamageTaken;
		Health.ZeroHealth += Die;
		
		Health.Initialize(SlimeStats.MaxHP);
		HitBox.Damage = SlimeStats.Attack;
		AddToGroup("Enemies");
	}

	public override void _PhysicsProcess(double delta)
	{
		var velocity = Velocity;

		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		if (_isHurting)
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, 15.0f * (float)delta);
			Velocity = velocity;
			MoveAndSlide();
			return;
		}
		
		if (IsInstanceValid(_player))
		{
			var direction = (_player.GlobalPosition - GlobalPosition).Normalized();
			velocity.X = direction.X * SlimeStats.Speed;
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _ExitTree()
	{
		_sprite.AnimationFinished -= OnAnimationFinished;
		Health.DamageTaken -= OnDamageTaken;
	}

	private void OnDamageTaken(Vector2 sourcePosition)
	{
		//GD.Print($"Slime take {amount} damage. {_currentHP} left");
		_sprite.Modulate = Colors.Red;
		_isHurting = true; // Change with state machine
		//Velocity = knockBackDir * 200;
		ResetHurtState();
	}
	
	private void Die()
	{
		var dt = new DeathData
		{
			Victim = this,
			Killer = null,
			DamageSourceId = null,
			Timestamp =  DateTimeOffset.Now.ToUnixTimeMilliseconds(),
			GoldReward = SlimeStats.Gold
		};
		EventBus.PublishEnemyDied(dt);
		_currentAnimation = "die";
		_sprite.Play(_currentAnimation);
	}

	private void OnAnimationFinished()
	{
		if (_currentAnimation == "die")
		{
			QueueFree();
		}
	}
	private void ResetHurtState()
	{
		GetTree().CreateTimer(0.2f).Timeout += () =>
		{
			if (!IsInstanceValid(this)) return;
			_sprite.Modulate = Colors.White;
			_isHurting = false;
		};
	}
}
