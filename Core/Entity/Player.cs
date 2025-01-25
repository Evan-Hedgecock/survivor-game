using System;
using Microsoft.Xna.Framework;
using Core.Systems;
using Core.Systems.States;

namespace Core.Entity;

public class Player(Rectangle bounds) : Character(bounds)
{
    public override Rectangle Hitbox
    {
        get
        {
            HitboxX = (int)(Bounds.X + (Bounds.Width * FacingDirection.X));
            HitboxY = (int)(CollisionBox.Y + (CollisionBox.Height * FacingDirection.Y));
            return new Rectangle(HitboxX, HitboxY, CollisionBox.Width * 4, CollisionBox.Height * 2);
            // return Hitbox;
        }
    }

    private State<Character> _damaged;
    private State<Character> _idle;
    private State<Character> _dying;
    private State<Character> _respawning;
    private State<Character> _attacking;

    public void Initialize()
    {
        // Movement properties
        Acceleration = 900;
        Friction = 900;
        Velocity = new Vector2(0, 0);
        MaxSpeed = 200;
        Speed = MaxSpeed;

        // Positional properties
        CollisionBoxHeight = Bounds.Height / 3;
        CollisionBoxY = CollisionBoxY + Bounds.Height - CollisionBox.Height;
        PositionX = BoundsX;
        PositionY = BoundsY;

        // Health and damage properties
        MaxHealth = 200;
        Health = MaxHealth;
        Damage = 50;

        HealthBar = new(5, 50, this);

        // Services
        _damageManager = Global.Services.GetService(typeof(DamageManager)) as DamageManager;
        InitializeCollisionManager();
        _timerManager = Global.Services.GetService(typeof(TimerManager)) as TimerManager;

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
        _timerManager.AddTimer(_attackCooldownTimer);
    }

    public void Update(Vector2 inputAxis, GameTime gameTime)
    {
        string states = string.Format("_invulnerable: {0}\n_alive: {1}\n_canAttack: {2}\n",
                                       _invulnerable, _alive, _canAttack);
        // Console.WriteLine(states);
        MoveAndSlide(inputAxis, gameTime);
        _stateMachine.Update(gameTime);
        _timerManager.Update(gameTime);
        HealthBar.Update();
    }
}
