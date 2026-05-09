
using CombatLab.Core.Data;
using CombatLab.Core.Events;
using CombatLab.Core.Interfaces;
using CombatLab.Presentation.Entities.Player.Components;
using CombatLab.Presentation.Entities.Player.States;
using CombatLab.Presentation.Strategies.Attack;
using GodotCombatLab.Core.FSM;
using Godot;


namespace CombatLab.Presentation.Entities.Player;

public partial class Player : Entity
{
    [Export] public PlayerStats Stats;
    [ExportGroup("Components")] [Export] public InputHandler Input { get; private set; }
    [Export] public PlayerStateMachine Fsm { get; private set; }

    [Export] public AnimationPlayer WeaponAnimator;
    
    [ExportGroup("Visuals")] [Export] public Sprite2D Sprite { get; private set; }
    [Export] public AnimationTree AnimTree { get; private set; }
    [Export] public Node2D WeaponPivot;
    

    [ExportGroup("Parameters")] [Export] public float JumpVelocity = -400.0f;

    public Vector2 KnockbackDirection { get; private set; }
    
    private AnimationNodeStateMachinePlayback _stateMachinePlayback;
    private float _currentHP;
   
    private IAttackStrategy _attackStrategy;

    public int FacingDirection { get; private set; } = 1;

    public override void _Ready()
    {
        if (Input == null) { GD.PushError("You must set InputHandler!") ; return; }
        if (Fsm == null) { GD.PushError("You must set FSM!"); return; }
        if (Stats == null) { GD.PushError("You must set Stats"); return;}

        Fsm.SetUp(this);

        if (AnimTree != null)
        {
            _stateMachinePlayback = (AnimationNodeStateMachinePlayback)AnimTree.Get("parameters/playback");
        }
        _attackStrategy = new MeleeAttack(10);
        _currentHP = Stats.MaxHP;
    }

    public override void _PhysicsProcess(double delta)
    {
        Fsm.UpdatePhysics(delta);

        if (!IsOnFloor())
        {
            Velocity += new Vector2(0, Gravity * (float)delta);
        }

        MoveAndSlide();

        UpdateFacing();
        UpdateWeaponRotation();
    }

    public override void _Process(double delta)
    {
        Fsm.UpdateInput(delta);
    }

// --- ACTIONS ---
    public void TryAttack()
    {
        if (Input.IsAttackJustPressed)
        {
            if (WeaponAnimator != null && !WeaponAnimator.IsPlaying())
            {
                Input.ConsumeAttack(); 
                UpdateWeaponRotation(true);
                WeaponAnimator.Play("swing"); 
            }
        }
    }
    
    public void ApplyMovement(float direction, double delta)
    {
        float targetSpeed = direction * Stats.Speed;
        bool changingDirection = direction * Velocity.X < 0;
        float accel = Mathf.IsZeroApprox(direction) ? Stats.Friction 
            : changingDirection ? Stats.Friction  
            : Stats.Acceleration;

        Velocity = new Vector2(
            Mathf.MoveToward(Velocity.X, targetSpeed, accel * (float)delta),
            Velocity.Y
        );
    }

    public void Jump()
    {
        Velocity = new Vector2(Velocity.X, Stats.JumpVelocity);
    }
    
    // --- Visuals ---
    public void UpdateFacing()
    {
        float mouseX = GetGlobalMousePosition().X;
        float playerX = GlobalPosition.X;
        
        FacingDirection = mouseX > playerX ? 1 : -1;

        Sprite.FlipH = FacingDirection == -1;
    }
    
    private void UpdateWeaponRotation(bool forceUpdate = false)
    {
        
        if (WeaponPivot == null) return;
        bool isAttacking = WeaponAnimator.IsPlaying() && WeaponAnimator.CurrentAnimation == "swing";

        if (isAttacking && !forceUpdate)
        {
            return;
        }
        Vector2 mousePos = GetGlobalMousePosition();
        Vector2 direction = mousePos - WeaponPivot.GlobalPosition;
        
        bool isLeft = mousePos.X < WeaponPivot.GlobalPosition.X;
        
        WeaponPivot.Scale = new Vector2(isLeft ? -1 : 1, 1);
        
        if (isLeft)
        {
            direction.X *= -1;
            WeaponPivot.Rotation = -direction.Angle();
        }
        else
        {
            WeaponPivot.Rotation = direction.Angle();
        }
        
    }

    public void TravelToAnimation(string stateName)
    {
        if (_stateMachinePlayback != null)
            _stateMachinePlayback.Travel(stateName);
    }

    public override void TakeDamage(int amount, Vector2 sourcePosition)
    {
        KnockbackDirection = (GlobalPosition - sourcePosition).Normalized();
        
        if (KnockbackDirection == Vector2.Zero)
            KnockbackDirection = new Vector2(-FacingDirection, -1);
        _currentHP -= amount;
        EventBus.PublishHealthChanged(_currentHP, Stats.MaxHP);
        GD.Print($"Took {amount} dmg, flying to {KnockbackDirection}");
        Fsm.ChangeState("playerhurt");
    }
    
    public void TakeDamage(int damage) 
    {
        TakeDamage(damage, Vector2.Zero);
    }
}