using CombatLab.Core.Events;
using Godot;
namespace CombatLab.Presentation.Entities.Player.Components;

[GlobalClass]
public partial class PlayerController : Node
{
    [Export] public Player Player;
    
    public void UpdateInput(double delta)
    {
        UpdateFacing();
        if (Player.PlayerInput.IsAttackJustPressed)
        {
            //GD.Print("Attack pressed!");
            Player.PlayerInput.ConsumeAttack();
            Player.Weapon.TryAttack();
        }
    }

    public void UpdatePhysics(double delta)
    {
        
    }
    public void ApplyMovement(float direction, double delta)
    {
        var targetSpeed = direction * Player.Stats.Speed;
        var changingDirection = direction * Player.Velocity.X < 0;
        var accel = Mathf.IsZeroApprox(direction) ? Player.Stats.Friction 
            : changingDirection ? Player.Stats.Friction  
            : Player.Stats.Acceleration;

        Player.Velocity = new Vector2(
            Mathf.MoveToward(Player.Velocity.X, targetSpeed, accel * (float)delta),
            Player.Velocity.Y
        );
    }
    
    public void Jump()
    {
        Player.Velocity = new Vector2(Player.Velocity.X, Player.Stats.JumpVelocity);
    }

    
    
    private void UpdateFacing()
    {
        var mouseX = Player.GetGlobalMousePosition().X;
        var playerX = Player.GlobalPosition.X;
        
        Player.FacingDirection = mouseX > playerX ? 1 : -1;

        Player.Sprite.FlipH = Player.FacingDirection == -1;
    }
}