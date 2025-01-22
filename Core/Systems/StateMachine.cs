using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Core.Systems;

public class StateMachine(State[] states, State initialState) {
    private readonly Dictionary<string, State> _states;
    private State _currentState;
    private readonly State _initialState = initialState;

    public void Initialize() {
        foreach (State state in states) {
            _states[state.Name] = state;
        }
        _initialState?.Enter();
    }
    public void Update(GameTime gameTime) {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _currentState?.Update(deltaTime);
    }

    public void ChangeState(string newState) {
        if (newState.Equals(_currentState.Name, StringComparison.CurrentCultureIgnoreCase)) {
            Console.WriteLine("New state is the current state");
            return;
        }
        _currentState?.Exit();
        if (_states[newState.ToLower()] != null) {
            _states[newState.ToLower()].Enter();
            _currentState = _states[newState.ToLower()];
        }
    }
}