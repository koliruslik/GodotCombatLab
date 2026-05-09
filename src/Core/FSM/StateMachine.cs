using Godot;
using System;
using System.Collections.Generic;

namespace GodotCombatLab.Core.FSM;

public abstract partial class StateMachine<T> : Node where T : Node
{
	[Export] public State<T> InitialState;

	protected Dictionary<(string state, string evt), string> _transitions = new();
	public State<T> _currentState { get; private set; }
	private Dictionary<string, State<T>> _states = new();

	protected abstract void RegisterTransitions();
	public void SetUp(T actor)
	{
		RegisterTransitions();
		foreach (var child in GetChildren())
		{
			if (child is State<T> state)
			{
				_states[state.StateName] = state;
				state.Init(actor);
				state.Transitioned += OnTransition;
			}
		}

		InitialState?.Enter();
		_currentState = InitialState;
	}

	public void UpdateInput(double delta)
	{
		_currentState?.Update(delta);
	}

	public void UpdatePhysics(double delta)
	{
		_currentState?.PhysicsUpdate(delta);
	}
	private void OnTransition(State<T> state, string eventName)
	{
		if (state != _currentState) return;
		var transKey = (state.StateName, eventName);
		if (_transitions.TryGetValue(transKey, out var nextState))
		{ 
			ChangeState(nextState);
		}
	}

	private void ChangeState(State<T> newState)
	{
		if (newState == _currentState) return;
		
		_currentState?.Exit();
		_currentState = newState;
		_currentState?.Enter();
	}
	
	public void ChangeState(string newStateName)
	{
		
		var key = newStateName;
		
		if (!_states.ContainsKey(key))
		{
			GD.PrintErr($"FSM: State '{newStateName}' not found!");
			return;
		}

		var newState = _states[key];
		
		if (newState == _currentState) return;
		
		_currentState?.Exit();
		_currentState = newState;
		_currentState?.Enter();
	}
	
}
