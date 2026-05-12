using Godot;
using System;
using CombatLab.Core.Utils;

namespace CombatLab.Core.FSM;

public partial class State<T> : Node where T : Node
{
    [Signal]
    public delegate void TransitionedEventHandler(State<T> state, string newStateName);

    protected T Actor;
    public virtual string StateName => GetType().Name;
    public virtual void Init(T actor)
    {
        Actor = actor;
    }

    public virtual void Enter()
    {
        GameLogger.Debug($"Entering {StateName}", LogCategory.State);
    }

    public virtual void Exit()
    {
        GameLogger.Debug($"Leaving {StateName}", LogCategory.State);
    }

    public virtual void Refresh()
    {
        GameLogger.Debug($"Refreshing {StateName}", LogCategory.State);
    }
    public virtual void Update(double delta) { }
    public virtual void PhysicsUpdate(double delta) { }
}
