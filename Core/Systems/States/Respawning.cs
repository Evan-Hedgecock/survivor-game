using System;
using System.Runtime.ConstrainedExecution;
using Core.Entity;

namespace Core.Systems.States;

public class Respawning<T>(string name, Character owner) : State<Character>(name, owner)
{
    public override void Enter()
    {
        // Play respawn animation
        // Start respawn timer
        // Call owner IsRespawning() for other functionality
        _owner.Speed = _owner.MaxSpeed;
        _owner.Health = _owner.MaxHealth;
        _owner.BoundsX = 0;
        _owner.BoundsY = 0;
    }

    public override void Exit()
    {
    }

    public override void Update(float deltaTime)
    {
        _owner.OnRespawn();
    }
}