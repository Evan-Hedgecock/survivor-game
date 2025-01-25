using System;
using Core.Entity;

namespace Core.Systems.States;

public class Attacking<T>(string name, Character owner) : State<Character>(name, owner)
{
    public override void Enter()
    {
        _owner.Attack();
    }

    public override void Exit()
    {
    }

    public override void Update(float deltaTime)
    {
    }
}