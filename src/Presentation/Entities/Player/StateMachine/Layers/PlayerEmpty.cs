using CombatLab.Core.FSM;
using CombatLab.Core.Utils;
using Godot;

namespace CombatLab.Presentation.Entities.Player.StateMachine.States;

[GlobalClass]
public partial class PlayerEmpty : State<Player>
{
    public override void Enter()
    {
        GameLogger.Debug($"Actor: {Actor}, Health: {Actor?.Health}", LogCategory.State);
        Actor.Health.DamageTaken += OnDamageTaken;
        Actor.Health.ZeroHealth += OnZeroHealth;
    }

    public override void Exit()
    {
        Actor.Health.DamageTaken -= OnDamageTaken;
        Actor.Health.ZeroHealth -= OnZeroHealth;
    }

    private void OnDamageTaken(Vector2 _)
    {
        EmitSignal(SignalName.Transitioned, this, "damaged");
    }

    private void OnZeroHealth()
    {
        EmitSignal(SignalName.Transitioned, this, "died");
    }
}