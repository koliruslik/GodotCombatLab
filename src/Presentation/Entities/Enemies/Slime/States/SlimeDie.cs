using CombatLab.Core.FSM;
using CombatLab.Core.Utils;
using Godot;

namespace CombatLab.Presentation.Entities.Enemies.SlimeStates;

[GlobalClass]
public partial class SlimeDie : State<Slime>
{
    public override void Enter()
    {
        GameLogger.Debug($"Entering {StateName}", LogCategory.State);
        Actor.PlayAnimation("die");
        Actor.Velocity = Vector2.Zero;
        Actor.HitBox.Disable();
        Actor.Sprite.AnimationFinished += OnDieFinished;
    }

    private void OnDieFinished()
    {
        Actor.QueueFree();
    }
    
    
}