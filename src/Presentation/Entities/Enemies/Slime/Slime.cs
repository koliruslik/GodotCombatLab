using Godot;
using System;
using CombatLab.Core.Data;
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
		if (SlimeStats == null) { GD.PushError("SlimeStats not set!"); return; }
		if(HitBox == null) { GD.PushError("HitBox not set!"); return; }
		if(HurtBox == null) { GD.PushError("HurtBox not set!"); return; }
		
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
		_sprite.AnimationFinished += OnAnimationFinished;
		AddToGroup("Enemies");
		_currentHP = SlimeStats.MaxHP;
		HitBox.Damage = SlimeStats.Attack;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		if (_isHurting)
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, 15.0f);
			Velocity = velocity;
			MoveAndSlide();
			return;
		}
		
		if (_player != null)
		{
			Vector2 direction = (_player.GlobalPosition - GlobalPosition).Normalized();
			velocity.X = direction.X * SlimeStats.Speed;
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}

	public override void TakeDamage(float amount, Vector2 sourcePosition)
	{
		_currentHP -= amount;
		GD.Print($"Slime take {amount} damage. {_currentHP} left");
		
		_sprite.Modulate = Colors.Red;
		_isHurting = true;
		
		//Velocity = knockBackDir * 200;
		if (_currentHP <= 0)
		{
			Die();
			return;
		}
		
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
	private async void ResetHurtState()
	{
		await ToSignal(GetTree().CreateTimer(0.2f), "timeout");
		if (!IsInstanceValid(this)) return;
		_sprite.Modulate = Colors.White;
		_isHurting = false;
	}
}
