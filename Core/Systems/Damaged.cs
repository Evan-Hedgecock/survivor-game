using System;
using Core.Entity;

namespace Core.Systems;

public class Damaged<T>(string name, Character owner) : State<Character>(name, owner)
{
    public override void Enter()
    {
        if (_owner.Health > 0) {
            _owner.OnDamage();
        } else {
            Console.WriteLine("Dead");
        }
    }

    public override void Exit()
    {
        Console.WriteLine("Exiting damaged state");
        _owner.OnRecover();
    }

    public override void Update(float deltaTime)
    {
        Console.WriteLine("Updating damaged state");
    }
}