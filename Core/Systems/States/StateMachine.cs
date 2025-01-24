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
        Console.Write("Current state: ");
        Console.WriteLine(_currentState.Name);
        _currentState?.Update(deltaTime);
    }

    public void ChangeState(string newState)
    {
        string changeState = string.Format("Current state: {0}\nNew state: {1}\n",
                                           _currentState.Name, newState);
        Console.WriteLine(changeState);
        Console.WriteLine("Inside change state");
        if (newState.Equals(_currentState.Name, StringComparison.CurrentCultureIgnoreCase))
        {
            Console.WriteLine("New state is the current state");
            return;
        }
        Console.WriteLine("About to exit current state");
        _currentState?.Exit();
        if (_states[newState.ToLower()] != null)
        {
            Console.WriteLine("Entering into new state");
            _states[newState.ToLower()].Enter();
            _currentState = _states[newState.ToLower()];
        }
        else
        {
            Console.WriteLine("New state was null");
        }
    }
}