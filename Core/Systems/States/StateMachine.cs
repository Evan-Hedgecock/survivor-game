using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Core.Systems.States;

public class StateMachine<T>(State<T>[] states, State<T> initialState)
{
    private readonly Dictionary<string, State<T>> _states = [];
    private State<T> _currentState;
    private readonly State<T> _initialState = initialState;

    public void Initialize()
    {
        foreach (State<T> state in states)
        {
            _states[state.Name] = state;
        }
        _initialState?.Enter();
        _currentState = _initialState;
    }
    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _currentState?.Update(deltaTime);
        Console.WriteLine(_currentState.Name);
    }

    public void ChangeState(string newState)
    {
        string changeState = string.Format("Current state: {0}\nNew state: {1}\n",
                                           _currentState.Name, newState);
        Console.WriteLine(changeState);
        if (newState.Equals(_currentState.Name, StringComparison.CurrentCultureIgnoreCase))
        {
            return;
        }
        _currentState?.Exit();
        if (_states[newState.ToLower()] != null)
        {
            _states[newState.ToLower()].Enter();
            _currentState = _states[newState.ToLower()];
        }
        else
        {
        }
    }
}