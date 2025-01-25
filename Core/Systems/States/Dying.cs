using System;
using Core.Entity;

namespace Core.Systems.States;

public class Dying<T>(string name, Character owner) : State<Character>(name, owner)
{
    public override void Enter()
    {
        // Perform dying animations, start death timer
        _owner.Speed = 0;
        _owner.OnDeath();
    }

    public override void Exit()
    {
        // Change state to respawn
    }

    public override void Update(float deltaTime)
    {
        _owner.Respawn();
    }
}