using CombatLab.Core.Events;
using CombatLab.Core.Interfaces;
using CombatLab.Core.Payloads;
using Godot;

namespace CombatLab.Presentation.Components;

[GlobalClass]
public partial class HealthComponent : Node, IDamageable
{
    [Signal] public delegate void ZeroHealthEventHandler();
    [Signal] public delegate void DamageTakenEventHandler(Vector2 sourcePosition);
    [Signal] public delegate void InvincibilityEndedEventHandler();
    
    [Export] public float InvincibilityTime = 0.5f;
    
    private float _invincibilityTimer;
    private float _currentHP;
    private float _maxHP;

    public void Initialize(float maxHP)
    {
        _maxHP = maxHP;
        _currentHP = _maxHP;
        EventBus.PublishHealthChanged(GetParent(), _currentHP, _maxHP);
    }

    public override void _Ready()
    {
        GetTree().ProcessFrame += OnFirstFrame;
    }

    public override void _Process(double delta)
    {
        if (_invincibilityTimer > 0)
        {
            _invincibilityTimer -= (float)delta;
            if(_invincibilityTimer <= 0) EmitSignal(SignalName.InvincibilityEnded);
        }
        
    }
    public void TakeDamage(float damage, Vector2 sourcePosition)
    {
        if (_currentHP <= 0) return;
        if (_invincibilityTimer > 0) return;
        _invincibilityTimer = InvincibilityTime;
        EmitSignal(SignalName.DamageTaken, sourcePosition);
        _currentHP = Mathf.Clamp(_currentHP - damage, 0, _maxHP);
        EventBus.PublishHealthChanged(GetParent(), _currentHP, _maxHP);
        if (_currentHP <= 0)
            EmitSignal(SignalName.ZeroHealth);
            
    }

    private void OnFirstFrame()
    {
        GetTree().ProcessFrame -= OnFirstFrame;
        EventBus.PublishHealthChanged(GetParent(), _currentHP, _maxHP);
    }
    
}