using System;

namespace Core.Systems.States;

public class Idle<T>(string name, T owner) : State<T>(name, owner)
{
    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Update(float deltaTime)
    {
    }
}