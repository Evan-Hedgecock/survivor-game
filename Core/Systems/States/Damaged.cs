using System;
using Core.Entity;

namespace Core.Systems.States;

public class Damaged<T>(string name, Character owner) : State<Character>(name, owner)
{
    public override void Enter()
    {
        if (_owner.Health > 0) {
            _owner.OnDamage();
        }
    }

    public override void Exit()
    {
        _owner.OnRecover();
    }

    public override void Update(float deltaTime)
    {
        Console.WriteLine("Updating damaged state");
    }
}