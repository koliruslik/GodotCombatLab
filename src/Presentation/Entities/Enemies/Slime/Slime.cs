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
		if (_player != null)
		{
			var direction = (_player.GlobalPosition - GlobalPosition).Normalized();
			velocity.X = direction.X * Stats.Speed;
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}
}

