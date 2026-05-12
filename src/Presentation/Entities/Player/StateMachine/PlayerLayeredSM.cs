using CombatLab.Presentation.Entities.Player.StateMachine.States;
using CombatLab.Presentation.Entities.Player.StateMachine.Layers;
using Godot;

namespace CombatLab.Presentation.Entities.Player.StateMachine;

[GlobalClass]
public partial class PlayerLayeredSM : Node
{
    [Export] public PlayerReactionsSM ReactionsSM { get; set; }
    [Export] public PlayerCombatSM CombatSM { get; set; }
    [Export] public PlayerMovementSM MovementSM { get; set; }

    public bool IsBusy => ReactionsSM.CurrentState is not PlayerEmpty;
    public void SetUp(Player player)
    {
        MovementSM.SetUp(player);
        CombatSM.SetUp(player);
        ReactionsSM.SetUp(player);
    }
    
    public void UpdatePhysics(double delta)
    {
        MovementSM.UpdatePhysics(delta);
        CombatSM.UpdatePhysics(delta);
        ReactionsSM.UpdatePhysics(delta);
    }

    public void UpdateInput(double delta)
    {
        MovementSM.UpdateInput(delta);
        CombatSM.UpdateInput(delta);
        ReactionsSM.UpdateInput(delta);
    }
}