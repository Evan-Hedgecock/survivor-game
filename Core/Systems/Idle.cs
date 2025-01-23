using System;

namespace Core.Systems;

public class Idle<T>(string name, T owner) : State<T>(name, owner)
{
    public override void Enter()
    {
        Console.WriteLine("Starting to idle");
    }

    public override void Exit()
    {
        Console.WriteLine("Exiting idle");
    }

    public override void Update(float deltaTime)
    {
        Console.WriteLine("Updating idle");
    }
}