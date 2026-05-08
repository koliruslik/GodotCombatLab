using Godot;
using System;
using CombatLab.Core.Payloads;
using CombatLab.entities.components;
using CombatLab.entities;
using CombatLab.Core.Events;

public partial class Slime : Entity, IDamageable
{
	private Node2D _player;
	
	private AnimatedSprite2D _sprite;
	private bool _isHurting = false;
	
	private string _currentAnimation = "";
	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
		_sprite.AnimationFinished += OnAnimationFinished;
		AddToGroup("Enemies");
		MaxHP = 50.0f;
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
			velocity.X = direction.X * Speed;
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}

	public void TakeDamage(int amount, Vector2 sourcePosition)
	{
		GD.Print($"Slime take {amount} damage. {MaxHP} left");
		
		MaxHP -= amount;
		
		_sprite.Modulate = Colors.Red;
		_isHurting = true;
		
		//Velocity = knockBackDir * 200;
		
		
		if (MaxHP <= 0)
		{
			Die();
			return;
		}

		if (IsInstanceValid(this))
		{
			_sprite.Modulate = Colors.White;
			_isHurting = false;
		}
	}

	private void Die()
	{
		var dt = new DeathData
		{
			Victim = this,
			Killer = null,
			DamageSourceId = null,
			Timestamp =  DateTimeOffset.Now.ToUnixTimeMilliseconds()
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
}
