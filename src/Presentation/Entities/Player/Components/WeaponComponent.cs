using CombatLab.Presentation.Components;
using CombatLab.Core.Data.Items.Weapons;
using CombatLab.Core.Interfaces;
using Godot;

namespace CombatLab.Presentation.Entities.Player.Components;

[GlobalClass]
public partial class WeaponComponent : Node2D
{
    [Export] public HitBox HitBox;
    [Export] public AnimationPlayer WeaponAnimator;
    [Export] public WeaponData WeaponData;

    private IAttackStrategy _attackStrategy;

    public override void _Ready()
    {
        if (HitBox == null) { GD.PushError("You must set HitBox first!"); return; }
        if (WeaponAnimator == null) { GD.PushError("You must set AnimationPlayer first!"); return; }
        if (WeaponData == null) { GD.PushError("You Must set Weapon Data  first!"); return; }
        _attackStrategy = WeaponData.WeaponType.ToStrategy(WeaponData.Damage);
        HitBox.Damage = WeaponData.Damage;
    }

    public void Update(double delta)
    {
        UpdateRotation();
    }
    public void EquipWeapon(WeaponData data)
    {
        WeaponData = data;
        _attackStrategy = WeaponData.WeaponType.ToStrategy(WeaponData.Damage);
    }
    
    public void TryAttack()
    {
        if (WeaponAnimator != null && !WeaponAnimator.IsPlaying())
        {
            UpdateRotation(true);
            WeaponAnimator.Play("swing"); 
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
}