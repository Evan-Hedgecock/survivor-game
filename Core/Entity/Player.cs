using Microsoft.Xna.Framework;
using Core.Systems;
using Core.Systems.States;

namespace Core.Entity;

public class Player(Rectangle bounds) : Character(bounds)
{
    private State<Character> _damaged;
    private State<Character> _idle;
    private State<Character> _dying;
    private State<Character> _respawning;

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
        Damage = 20;

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
        _stateMachine = new StateMachine<Character>([_damaged, _idle, _dying, _respawning], _idle);
        _stateMachine.Initialize();

        // Timers
        _invulnerableTimer = new Timer(0.5f, DamageTimerAlarm);
        _timerManager.AddTimer(_invulnerableTimer);
    }

    public void Update(Vector2 inputAxis, GameTime gameTime)
    {
        MoveAndSlide(inputAxis, gameTime);
        _stateMachine.Update(gameTime);
        _timerManager.Update(gameTime);
        HealthBar.Update();
    }
}
