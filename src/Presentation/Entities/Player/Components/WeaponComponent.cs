using CombatLab.Presentation.Components;
using CombatLab.Core.Data.Items.Weapons;
using CombatLab.Core.Interfaces;
using Godot;

namespace CombatLab.Presentation.Entities.Player.Components;

[GlobalClass]
public partial class WeaponComponent : Node2D, IAttacker
{
    [Export] public HitBox HitBox;
    [Export] public AnimationPlayer WeaponAnimator;
    [Export] public WeaponData WeaponData;

    private IAttackStrategy _attackStrategy;
    private StringName _attackAnimName = null;

    public override void _Ready()
    {
        if (HitBox == null) { GD.PushError("You must set HitBox first!"); return; }
        if (WeaponAnimator == null) { GD.PushError("You must set AnimationPlayer first!"); return; }
        if (WeaponData == null) { GD.PushError("You Must set Weapon Data  first!"); return; }
        
        WeaponAnimator.AnimationFinished += OnAttackFinished;
        HitBox.HitDetected += OnHitDetected;
        
        _attackAnimName = "swing";
        UpdateWeaponStats(WeaponData);
    }

    public override void _ExitTree()
    {
        WeaponAnimator.AnimationFinished -= OnAttackFinished;
        HitBox.HitDetected -= OnHitDetected;
    }

    public void Update(double delta)
    {
        UpdateRotation();
    }
    public void EquipWeapon(WeaponData data)
    {
        WeaponData = data;
        UpdateWeaponStats(data);
    }
    
    public void TryAttack()
    {
        if (WeaponAnimator != null && !WeaponAnimator.IsPlaying())
        {
            UpdateRotation(true);
            WeaponAnimator.Play(_attackAnimName); 
        }
    }
    
    private void UpdateRotation(bool forceUpdate = false)
    {
        var isAttacking = WeaponAnimator.IsPlaying() && WeaponAnimator.CurrentAnimation == "swing";

        if (isAttacking && !forceUpdate)
        {
            return;
        }
        var mousePos = GetGlobalMousePosition();
        var direction = mousePos - GlobalPosition;
        
        var isLeft = mousePos.X < GlobalPosition.X;
        
        Scale = new Vector2(isLeft ? -1 : 1, 1);
        
        if (isLeft)
        {
            direction.X *= -1;
            Rotation = -direction.Angle();
        }
        else
        {
            Rotation = direction.Angle();
        }
        
    }

    private void UpdateWeaponStats(WeaponData data)
    {
        _attackStrategy = WeaponData.WeaponType.ToStrategy(WeaponData.Damage);
    }

    private void OnAttackFinished(StringName animName)
    {
        if(animName == _attackAnimName)
            HitBox.ResetHits();
    }

    private void OnHitDetected(Node victim)
    {
        //GD.Print("Hit!");
        if(victim is IDamageable target)
        _attackStrategy.Execute(this, target);
    }
}