using Godot;
using System;
using CombatLab.Core.Data.Entities;
using CombatLab.Core.Events;
using CombatLab.Core.Interfaces;
using CombatLab.Core.Payloads;
using CombatLab.Presentation.Components;
using CombatLab.Presentation.Strategies.Attack;

namespace CombatLab.Presentation.Entities.Enemies;

public partial class Slime : Enemy
{
	public override void _PhysicsProcess(double delta)
	{
		var velocity = Velocity;

		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		// if (_isHurting)
		// {
		//     velocity.X = Mathf.MoveToward(Velocity.X, 0, 15.0f * (float)delta);
		//     Velocity = velocity;
		//     MoveAndSlide();
		//     return;
		// }
		
		if (IsInstanceValid(_player) && _player is Node2D playerNode)
		{
			var direction = (playerNode.GlobalPosition - GlobalPosition).Normalized();
			velocity.X = direction.X * Stats.Speed;
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}
}
