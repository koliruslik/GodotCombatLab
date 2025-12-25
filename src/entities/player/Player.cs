using CombatLab.entities.player.components;
using CombatLab.entities.player.States;
using Godot;

namespace CombatLab.entities.player;

public partial class Player : Entity
{
    [ExportGroup("Components")]
    [Export] public InputHandler Input { get; private set; }
    [Export] public PlayerStateMachine Fsm { get; private set; }
    
    [ExportGroup("Visuals")]
    [Export] public Sprite2D Sprite { get; private set; }
    [Export] public AnimationTree AnimTree { get; private set; }
    [Export] public Marker2D WeaponSlot { get; private set; }
    
    [ExportGroup("Parameters")]
    [Export] public float JumpVelocity = -400.0f;

    private AnimationNodeStateMachinePlayback _stateMachinePlayback;
    
    public int FacingDirection { get; private set; } = 1;
    public override void _Ready()
    {
        if(Input == null) GD.PushError("You must set InputHandler!");
        if(Fsm == null) GD.PushError("You must set FSM!");
        
        Fsm.SetUp(this);

        if (AnimTree != null)
        {
            _stateMachinePlayback = (AnimationNodeStateMachinePlayback)AnimTree.Get("parameters/playback");
        }
    }

    public void ApplyGravity(double delta)
    {
        if(!IsOnFloor())
            Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity * (float)delta);
    }
    
    public void HandleMovement(float targetXVelocity, float acceleration, double delta)
    {
        Velocity = new Vector2(
            Mathf.MoveToward(Velocity.X, targetXVelocity, acceleration * (float)delta),
            Velocity.Y
        );
        
        MoveAndSlide();
    }
    
    public void UpdateFacing(float moveInput)
    {
        if (Mathf.IsZeroApprox(moveInput)) return;
        
        FacingDirection = moveInput > 0 ? 1 : -1;
        
        Sprite.FlipH = FacingDirection == -1;
    }

    public void TravelToAnimation(string stateName)
    {
        if (_stateMachinePlayback == null) return;
        
        _stateMachinePlayback.Travel(stateName);
    }
    
    public void Jump()
    {
        Velocity = new Vector2(Velocity.X, JumpVelocity);
        Fsm.ChangeState("playerair");
    }
}