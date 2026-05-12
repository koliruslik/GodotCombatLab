using Godot;
using CombatLab.Core.FSM;
using CombatLab.Core.Interfaces;
using CombatLab.Core.Utils;

namespace CombatLab.Presentation.Entities.Player.StateMachine.States;

[GlobalClass]
public partial class PlayerHurt : State<Player>
{
    [Export] public float StunDuration = 0.3f;

    private float _timer;
    private bool _animationFinished = false;

    public override void Enter()
    {
        base.Enter();
        _animationFinished = false;
        _timer = StunDuration;
        Actor.PlayAnimation("hurt");
        Actor.Sprite.AnimationFinished += OnAnimationFinished;
        Actor.Health.ZeroHealth += OnZeroHealth;
        float frameCount = Actor.Sprite.SpriteFrames.GetFrameCount("hurt");
        float fps = (float)Actor.Sprite.SpriteFrames.GetAnimationSpeed("hurt");
        float animLength = frameCount / fps;
        Actor.Sprite.SpeedScale = animLength / StunDuration;
        var dir = (Actor.GlobalPosition - Actor.LastHitSourcePosition).Normalized();
        Actor.Velocity = new Vector2(dir.X * Actor.LastKnockbackForce, -Actor.LastKnockbackLift);
    }

    public override void Exit()
    {
        base.Exit();
        Actor.Sprite.SpeedScale = 1f;
        Actor.Sprite.AnimationFinished -= OnAnimationFinished;
        Actor.Health.ZeroHealth -= OnZeroHealth;
    }

    public override void PhysicsUpdate(double delta)
    {
        _timer -= (float)delta;
        if (_timer <= 0 && _animationFinished)
        {
            _animationFinished = false;
            _timer = StunDuration;
            EmitSignal(SignalName.Transitioned, this, "finished");
        }
    }

    private void OnAnimationFinished()
    {
        GameLogger.Debug($"AnimFinished: {Actor.Sprite.Animation}", LogCategory.State);
        _animationFinished = true;
    }
    
    private void OnZeroHealth()
    {
        EmitSignal(SignalName.Transitioned, this, "died");
    }
    
}