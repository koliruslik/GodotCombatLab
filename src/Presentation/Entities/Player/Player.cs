
using System;
using CombatLab.Core.Data.Entities;
using CombatLab.Core.Events;
using CombatLab.Core.Interfaces;
using CombatLab.Core.Payloads;
using CombatLab.Presentation.Entities.Player.Components;
using CombatLab.Presentation.Entities.Player.States;
using CombatLab.Presentation.Strategies.Attack;
using Godot;


namespace CombatLab.Presentation.Entities.Player;

public partial class Player : Entity
{
    [Export] public PlayerStats Stats;
    [ExportGroup("Components")]
    [Export] public PlayerController Controller;
    [Export] public WeaponComponent Weapon;
    [Export] public InputHandler PlayerInput { get; private set; }
    [Export] public PlayerStateMachine Fsm { get; private set; }

    
    [ExportGroup("Visuals")] 
    [Export] public Sprite2D Sprite { get; private set; }
    [Export] public AnimationTree AnimTree { get; private set; }
    
    public Vector2 KnockbackDirection { get; private set; }
    public int FacingDirection { get; set; } = 1;
    
    private AnimationNodeStateMachinePlayback _stateMachinePlayback;
    
    private const string AnimPlaybackPath = "parameters/playback";

    public override void _Ready()
    {
        base._Ready();
        AddToGroup("Player");
        if (PlayerInput == null) { GD.PushError("You must set InputHandler!") ; return; }
        if (Fsm == null) { GD.PushError("You must set FSM!"); return; }
        if (Stats == null) { GD.PushError("You must set Stats"); return;}
        if (Controller == null) {  GD.PushError("You must set Controller!"); return; }
        if (Weapon == null) { GD.PushError("You must set WeaponComponent!"); return; }
        Fsm.SetUp(this);

        if (AnimTree != null)
        {
            _stateMachinePlayback = (AnimationNodeStateMachinePlayback)AnimTree.Get(AnimPlaybackPath);
        }

        Health.DamageTaken += OnDamageTaken;
        Health.ZeroHealth += PlayerDie;
        
        Health.Initialize(Stats.MaxHP);
    }
    
    public override void _Process(double delta)
    {
        Fsm.UpdateInput(delta);
        Controller.UpdateInput(delta); 
    }
    public override void _PhysicsProcess(double delta)
    {
        Fsm.UpdatePhysics(delta);

        if (!IsOnFloor())
        {
            Velocity += new Vector2(0, Gravity * (float)delta);
        }

        MoveAndSlide();

        Controller.UpdatePhysics(delta);
        Weapon.Update(delta);
    }

    public override void _ExitTree()
    {
        Health.DamageTaken -= OnDamageTaken;
    }
    
    public void TravelToAnimation(string stateName)
    {
        if (_stateMachinePlayback != null)
            _stateMachinePlayback.Travel(stateName);
    }

    private void OnDamageTaken(Vector2 sourcePosition)
    {
        KnockbackDirection = (GlobalPosition - sourcePosition).Normalized();
        //GD.Print($"Took {amount} dmg, flying to {KnockbackDirection}");
        Fsm.ChangeState("playerhurt");
    }
    
    private void PlayerDie()
    {
        GD.Print("Player Died");
        var dt = new DeathData
        {
            Victim = this,
            Killer = null,
            DamageSourceId = "",
            Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds()
        };
        EventBus.PublishPlayerDeath(dt);
    }
}