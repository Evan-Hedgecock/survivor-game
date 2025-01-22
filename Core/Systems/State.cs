using System;
using Microsoft.Xna.Framework;

namespace Core.Systems;

public abstract class State {
    public string Name {
        get { return _name.ToLower(); }
        set { _name = value.ToLower(); }
    }
    private string _name;
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update(float deltaTime);
}