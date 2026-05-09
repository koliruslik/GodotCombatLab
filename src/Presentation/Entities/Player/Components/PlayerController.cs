using Godot;
namespace CombatLab.Presentation.Entities.Player.Components;

[GlobalClass]
public partial class PlayerController : Node
{
    [Export] public Player Player;
    public void TryAttack()
    {
        if (Player.Input.IsAttackJustPressed)
        {
            if (Player.WeaponAnimator != null && !Player.WeaponAnimator.IsPlaying())
            {
                Player.Input.ConsumeAttack(); 
                UpdateWeaponRotation(true);
                Player.WeaponAnimator.Play("swing"); 
            }
        }
    }
    
    public void ApplyMovement(float direction, double delta)
    {
        float targetSpeed = direction * Player.Stats.Speed;
        bool changingDirection = direction * Player.Velocity.X < 0;
        float accel = Mathf.IsZeroApprox(direction) ? Player.Stats.Friction 
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
    
    public void UpdateFacing()
    {
        float mouseX = Player.GetGlobalMousePosition().X;
        float playerX = Player.GlobalPosition.X;
        
        Player.FacingDirection = mouseX > playerX ? 1 : -1;

        Player.Sprite.FlipH = Player.FacingDirection == -1;
    }
    
    public void UpdateWeaponRotation(bool forceUpdate = false)
    {
        
        if (Player.WeaponPivot == null) return;
        bool isAttacking = Player.WeaponAnimator.IsPlaying() && Player.WeaponAnimator.CurrentAnimation == "swing";

        if (isAttacking && !forceUpdate)
        {
            return;
        }
        Vector2 mousePos = Player.GetGlobalMousePosition();
        Vector2 direction = mousePos - Player.WeaponPivot.GlobalPosition;
        
        bool isLeft = mousePos.X < Player.WeaponPivot.GlobalPosition.X;
        
        Player.WeaponPivot.Scale = new Vector2(isLeft ? -1 : 1, 1);
        
        if (isLeft)
        {
            direction.X *= -1;
            Player.WeaponPivot.Rotation = -direction.Angle();
        }
        else
        {
            Player.WeaponPivot.Rotation = direction.Angle();
        }
        
    }
}