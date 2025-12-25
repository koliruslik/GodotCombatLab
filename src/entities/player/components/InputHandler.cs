using Godot;

namespace CombatLab.entities.player.components;

[GlobalClass]
public partial class InputHandler : Node
{
    public Vector2 MoveDirection { get; private set; }
    
    public bool IsJumpJustPressed { get; private set; }
    public bool IsAttackJustPressed { get; private set; }
    public bool IsAttackHeld { get; private set; }
    
    [Export] public string ActionLeft = "moveLeft";
    [Export] public string ActionRight = "moveRight";
    [Export] public string ActionUp = "moveUp";
    [Export] public string ActionDown = "moveDown";
    [Export] public string ActionAttack = "attack";
    [Export] public string ActionJump = "jump";

    public override void _Process(double delta)
    {
        MoveDirection = Input.GetVector(ActionLeft, ActionRight, ActionUp, ActionDown);
        
        IsAttackJustPressed = Input.IsActionJustPressed(ActionAttack);
        IsAttackHeld = Input.IsActionPressed(ActionAttack);
        IsJumpJustPressed = Input.IsActionJustPressed(ActionJump);
    }
    
    public void ConsumeAttack() => IsAttackJustPressed = false;
}