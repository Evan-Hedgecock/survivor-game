using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Core.Systems;

public class StateMachine<T>(State<T>[] states, State<T> initialState) {
    private readonly Dictionary<string, State<T>> _states = [];
    private State<T> _currentState;
    private readonly State<T> _initialState = initialState;

    public void Initialize() {
        foreach (State<T> state in states) {
            _states[state.Name] = state;
        }
        _initialState?.Enter();
        _currentState = _initialState;
    }
    public void Update(GameTime gameTime) {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _currentState?.Update(deltaTime);
    }

    public void ChangeState(string newState) {
        Console.WriteLine("Inside change state");
        if (newState.Equals(_currentState.Name, StringComparison.CurrentCultureIgnoreCase)) {
            Console.WriteLine("New state is the current state");
            return;
        }
        Console.WriteLine("About to exit current state");
        _currentState?.Exit();
        if (_states[newState.ToLower()] != null) {
            _states[newState.ToLower()].Enter();
            _currentState = _states[newState.ToLower()];
        }
    }
}