using Godot;
using System;
using System.Collections.Generic;

public partial class StateMachine<T> : Node where T : Node
{
	[Export] public State<T> InitialState; 
	public State<T> _currentState { get; private set; }
	private Dictionary<string, State<T>> _states = new();

	public void SetUp(T actor)
	{
		foreach (var child in GetChildren())
		{
			if (child is State<T> state)
			{
				_states[child.Name.ToString().ToLower()] = state;
				state.Init(actor);
				state.Transitioned += OnTransition;
			}
		}

		InitialState?.Enter();
		_currentState = InitialState;
	}

	public override void _Process(double delta) => _currentState?.Update(delta);
	public override void _PhysicsProcess(double delta) => _currentState?.PhysicsUpdate(delta);

	private void OnTransition(State<T> state, string newStateName)
	{
		if (state != _currentState) return;
		
		var newState = _states.GetValueOrDefault(newStateName.ToLower());
		if(newState != null) ChangeState(newState);
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
		
		var key = newStateName.ToLower();
		
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
