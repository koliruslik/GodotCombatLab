using Godot;
using System;

namespace CombatLab.entities.player;
public partial class Player : CharacterBody2D
{
	[Export] public float Speed = 300.0f;
	[Export] public float JumpVelocity = -400.0f;

	private bool _isOnFloor = true;
	private bool _isAttacking = false;

	private Animator _animator;
	
	public override void _Ready()
	{
		_isOnFloor = IsOnFloor();
		_animator = new Animator(
			GetNode<AnimatedSprite2D>("AnimatedSprite2D"),
			true
			);
	}

	public override void _PhysicsProcess(double delta)
	{
		var direction = Input.GetVector("moveLeft", "moveRight", 
			"moveUp", "moveDown");
		Velocity = CalculateVelocity(delta, direction);
		_animator.Update(Velocity, direction, _isOnFloor, _isAttacking);
		MoveAndSlide();
	}

	private Vector2 CalculateVelocity(double delta, Vector2 direction)
	{
		var velocity = Velocity;
		_isOnFloor = IsOnFloor();
		
		if (!_isOnFloor)
		{
			velocity += GetGravity() * (float)delta;
		}
		
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}
		
		return velocity;
	}

}
