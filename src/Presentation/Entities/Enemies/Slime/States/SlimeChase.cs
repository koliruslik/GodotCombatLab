using Godot;
using CombatLab.Core.FSM;
using CombatLab.Core.Utils;

namespace CombatLab.Presentation.Entities.Enemies.SlimeStates;

[GlobalClass]
public partial class SlimeChase : State<Slime>
{
    private float _jumpTimer = 0f;
    private bool _wasInAir = false;
    public override void PhysicsUpdate(double delta)
    {
        _jumpTimer -= (float)delta;
        if (Actor.IsOnFloor() && _jumpTimer <= 0f && Actor.Player != null)
        {
            _jumpTimer = Actor.Stats.AttackCooldown;
            var direction = (Actor.Player.GlobalPosition - Actor.GlobalPosition).Normalized();
            Actor.Velocity = new Vector2(direction.X * Actor.Stats.Speed, -Actor.Stats.JumpVelocity);
        }
        else if(Actor.IsOnFloor())
        {
            Actor.Velocity = new Vector2(
                Mathf.MoveToward(Actor.Velocity.X, 0, Actor.Stats.Speed),
                Actor.Velocity.Y
            );
        }

        if (_wasInAir && Actor.IsOnFloor())
        {
            Actor.PlayAnimation("land");
            _wasInAir = false;
        }
        else if (Actor.Velocity.Y < 0)
        {
            _wasInAir = true;
            Actor.PlayAnimation("jump");
        }
        else if (Actor.Velocity.Y > 0)
        {
            _wasInAir = true;
            Actor.PlayAnimation("fall");
        }


        if (Actor.IsOnFloor() && !Actor.IsPlayerInDetecionRange())
            EmitSignal(SignalName.Transitioned, this, "playerLost");
    }
}