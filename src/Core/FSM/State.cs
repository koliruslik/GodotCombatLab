using Godot;
using System;



public partial class State<T> : Node where T : Node
{
    [Signal]
    public delegate void TransitionedEventHandler(State<T> state, string newStateName);

    protected T Actor;

    public virtual void Init(T actor)
    {
        Actor = actor;
    }
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update(double delta) { }
    public virtual void PhysicsUpdate(double delta) { }
}
