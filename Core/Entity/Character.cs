using Microsoft.Xna.Framework;
using Core.Objects;
using Core.Systems;
using Core.Systems.States;
using System;
using Core.UI;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Linq.Expressions;

namespace Core.Entity;

public class Character(Rectangle bounds) : PhysicsObject(bounds)
{
    public virtual Rectangle Hurtbox
    {
        get
        {
            return new Rectangle(CollisionBoxX - 2, CollisionBoxY - 2,
                                 CollisionBox.Width + 4,
                                 CollisionBox.Height + 4);
        }
    }
    public virtual Rectangle Hitbox
    {
        get
        {
            return new Rectangle(CollisionBoxX, CollisionBoxY - 2,
                                 CollisionBox.Width,
                                 CollisionBox.Height + 2);
        }
        set
        {
            _hitBox = value;
        }
    }
    public virtual int HitboxX
    {
        get
        {
            return _hitBox.X;
        }
        set
        {
            _hitBox.X = value;
        }
    }
    public virtual int HitboxY
    {
        get
        {
            return _hitBox.Y;
        }
        set
        {
            _hitBox.Y = value;
        }
    }
    private Rectangle _hitBox;
    public int Damage;
    public int Health;
    public int MaxHealth;
    protected TimerManager _timerManager;
    protected DamageManager _damageManager;
    protected StateMachine<Character> _stateMachine;
    protected bool _invulnerable;
    protected bool _alive = true;
    protected bool _canAttack = true;
    protected Timer _invulnerableCooldownTimer;
    protected Timer _attackCooldownTimer;
    public HealthBar HealthBar;

    public void TakeDamage(int amount)
    {
        if (_invulnerable || !_alive)
        {
            return;
        }
        Health -= amount;
        if (Health > 0)
        {
            _stateMachine.ChangeState("damaged");
        }
        else
        {
            _stateMachine.ChangeState("dying");
        }
    }
    public void OnDamage()
    {
        _invulnerableCooldownTimer.Start();
        _invulnerable = true;
    }
    public void OnRecover()
    {
        _invulnerable = false;
    }
    public void OnDeath()
    {
        _alive = false;
    }
    public void Respawn()
    {
        _stateMachine.ChangeState("respawning");
    }
    public void OnRespawn()
    {
        _alive = true;
        _stateMachine.ChangeState("idle");
    }
    public void InvulnerableCooldownTimerAlarm()
    {
        _stateMachine.ChangeState("idle");
    }
    public void AttackCooldownTimerAlarm() {
        _canAttack = true;
        _stateMachine.ChangeState("idle");
    }
    public void StartAttack() {
        _attackCooldownTimer.Start();
        if (_canAttack) _stateMachine.ChangeState("attacking");
    }
    public void Attack() {
        _canAttack = false;
        string attackInfo = string.Format("Hitbox: {0}\nBounds: {1}\nCollisionBox: {2}",
                                          Hitbox, Bounds, CollisionBox);
        _damageManager.IsDamaging(this);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        HealthBar.Draw(spriteBatch);
    }
}