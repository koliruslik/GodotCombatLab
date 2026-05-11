
using System;
using CombatLab.Core.Data.Entities;
using CombatLab.Core.Events;
using CombatLab.Core.Interfaces;
using CombatLab.Core.Payloads;
using CombatLab.Core.Services;
using CombatLab.Presentation.Entities.Player.Components;
using CombatLab.Presentation.Entities.Player.States;
using CombatLab.Presentation.Strategies.Attack;
using Godot;
using CombatLab.Core.Utils;


namespace CombatLab.Presentation.Entities.Player;

public partial class Player : Entity, IPlayer
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
    
    public event Action OnInvincibilityEnded;
    
    public Vector2 KnockbackDirection { get; private set; }
    public int FacingDirection { get; set; } = 1;
    
    private AnimationNodeStateMachinePlayback _stateMachinePlayback;
    
    private const string AnimPlaybackPath = "parameters/playback";

    public override void _Ready()
    {
        base._Ready();
        if (PlayerInput == null) { GameLogger.Error("You must set InputHandler!") ; return; }
        if (Fsm == null) { GameLogger.Error("You must set FSM!"); return; }
        if (Stats == null) { GameLogger.Error("You must set Stats"); return;}
        if (Controller == null) {  GameLogger.Error("You must set Controller!"); return; }
        if (Weapon == null) { GameLogger.Error("You must set WeaponComponent!"); return; }
        Fsm.SetUp(this);

        if (AnimTree != null)
        {
            _stateMachinePlayback = (AnimationNodeStateMachinePlayback)AnimTree.Get(AnimPlaybackPath);
        }

        Health.DamageTaken += OnDamageTaken;
        Health.ZeroHealth += PlayerDie;
        Health.InvincibilityEnded += () => OnInvincibilityEnded?.Invoke();
        
        Health.Initialize(Stats.MaxHP);
        
        ServiceLocator.Register<IPlayer>(this);
        AddToGroup("Player");
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
        ServiceLocator.Unregister<IPlayer>();
        GameLogger.Info("Player has been exited");
    }
    
    public void TravelToAnimation(string stateName)
    {
        if (_stateMachinePlayback != null)
            _stateMachinePlayback.Travel(stateName);
    }
    
 

    private void OnDamageTaken(Vector2 sourcePosition)
    {
        KnockbackDirection = (GlobalPosition - sourcePosition).Normalized();
        Fsm.ChangeState("playerhurt");
    }
    
    private void PlayerDie()
    {
        GameLogger.Info("Player Died");
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