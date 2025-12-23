using Godot;

namespace CombatLab.entities.player;

public class Animator
{
    private readonly AnimatedSprite2D _mainSprite;
    private bool _mouseMode = false;
    public Animator(AnimatedSprite2D mainSprite, bool mouseMode)
    {
        _mainSprite = mainSprite;
        _mouseMode = mouseMode;
    }
    public void Update(Vector2 velocity, Vector2 direction, bool isOnFloor, bool isAttacking)
    {
        HandleRotation(_mouseMode);
        var currentAnimationName = Anims.Idle; 
        if (!isOnFloor)
            currentAnimationName = velocity.Y < 0 ? Anims.Jump : Anims.Fall;
        else if (velocity != Vector2.Zero)
            currentAnimationName = Anims.Walk;

        if(_mainSprite.Animation != currentAnimationName)
            _mainSprite.Play(currentAnimationName);
    }

    private void HandleRotation(bool mouseMod)
    {
        if (mouseMod)
        {
            Vector2 mousePos = _mainSprite.GetLocalMousePosition();
            
            bool lookLeft = mousePos.X < 0;
            _mainSprite.FlipH = lookLeft;
            //_effectSprite.FlipV = lookLeft; 
        
            //_effectSprite.Rotation = mousePos.Angle();
        }
        else
        {
            return;
        }
    }
}