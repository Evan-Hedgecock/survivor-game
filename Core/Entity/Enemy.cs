using Microsoft.Xna.Framework;
using Core.Systems;
using Core.Systems.States;
using System.Collections.Generic;
using System;

namespace Core.Entity;
public class Enemy(Rectangle bounds, Grid grid) : Character(bounds)
{
    private Pathfinder _pathfinder;
    private List<Vector2> _path;
    private readonly Grid _grid = grid;
    private State<Character> _damaged;
    private State<Character> _idle;
    private State<Character> _dying;
    private State<Character> _respawning;
    private State<Character> _attacking;

    public void Initialize()
    {
        // Movement properties
        Acceleration = 200;
        Friction = 50;
        Velocity = new Vector2(0, 0);
        MaxSpeed = 50;
        Speed = MaxSpeed;
        // Positional properties
        CollisionBoxHeight = Bounds.Height / 3;
        CollisionBoxY = CollisionBoxY + Bounds.Height - CollisionBox.Height;
        PositionX = BoundsX;
        PositionY = BoundsY;

        // Health and damage properties
        MaxHealth = 100;
        Health = MaxHealth;
        Damage = 10;
        HealthBar = new(3, 20, this);

        // Services
        _pathfinder = Global.Services.GetService(typeof(Pathfinder)) as Pathfinder;
        _damageManager = Global.Services.GetService(typeof(DamageManager)) as DamageManager;
        _timerManager = Global.Services.GetService(typeof(TimerManager)) as TimerManager;
        InitializeCollisionManager();

        // States
        _damaged = new Damaged<Character>("damaged", this);
        _idle = new Idle<Character>("idle", this);
        _dying = new Dying<Character>("dying", this);
        _respawning = new Respawning<Character>("respawning", this);
        _attacking = new Attacking<Character>("attacking", this);
        _stateMachine = new StateMachine<Character>([_damaged, _idle, _dying, _respawning, _attacking], _idle);
        _stateMachine.Initialize();

        // Timers
        _invulnerableCooldownTimer = new Timer(0.5f, InvulnerableCooldownTimerAlarm);
        _attackCooldownTimer = new(1f, AttackCooldownTimerAlarm);
        _timerManager.AddTimer(_invulnerableCooldownTimer);
    }

    public void Update(Player player, GameTime gameTime)
    {
        Node target = _grid.WorldPosToNode(new Vector2(player.CollisionBoxX,
                                                       player.CollisionBoxY));
        Node start = _grid.WorldPosToNode(new Vector2(CollisionBoxX,
                                                      CollisionBoxY));
        _path = _pathfinder.FindPath(start, target);
        MoveAndSlide(MoveDirection(player), gameTime);
        if (_damageManager.IsDamaging(this))
        {
            Console.WriteLine("Is damaging");
        }
        HealthBar.Update();
        _stateMachine.Update(gameTime);
    }

    private Vector2 MoveDirection(Player player)
    {
        Vector2 direction;
        try
        {
            direction = Vector2.Subtract(_path[1], _path[0]);
        }
        catch (Exception)
        {
            direction = Vector2.Subtract(player.Position, Position);
        }
        return direction;
    }
}