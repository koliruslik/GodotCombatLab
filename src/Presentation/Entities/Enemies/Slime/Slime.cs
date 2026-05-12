using CombatLab.Core.Interfaces;
using CombatLab.Presentation.Entities.Enemies.SlimeStates;
using Godot;

namespace CombatLab.Presentation.Entities.Enemies;

public partial class Slime : Enemy, IKnockbackable
{
	[Export] public SlimeStateMachine Fsm { get; private set; }
	
	public Vector2 LastHitSourcePosition { get; set; }
	public float LastKnockbackForce { get; set; }
	public override void _Ready()
	{
		base._Ready();
		Fsm.SetUp(this);
	}

	public override void _Process(double delta)
	{
		Fsm.UpdateInput(delta);
	}
	public override void _PhysicsProcess(double delta)
	{
		var velocity = Velocity;
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		Velocity = velocity;
		Fsm.UpdatePhysics(delta);
		MoveAndSlide();
	}
	
	public void ApplyKnockback(Vector2 sourcePosition, float force)
	{
		LastHitSourcePosition = sourcePosition;
		LastKnockbackForce = force;
		Fsm.OnHurt();
	}
}

