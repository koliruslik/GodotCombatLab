using Godot;
using System;

public partial class Slime : CharacterBody2D
{
	[Export] public int Health = 30;
	[Export] public float Speed = 50.0f;

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

	public async void TakeDamage(int amount, Vector2 knockBackDir)
	{
		GD.Print($"Slime take {amount} damage. {Health} left");
		
		Health -= amount;
		
		_sprite.Modulate = Colors.Red;
		_isHurting = true;
		
		Velocity = knockBackDir * 200;

		await ToSignal(GetTree().CreateTimer(.25f), SceneTreeTimer.SignalName.Timeout);
		
		if (Health <= 0)
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
		GD.Print("Slime Dead!");
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
