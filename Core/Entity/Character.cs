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

public class Character(Rectangle bounds) : PhysicsObject(bounds) {
    public virtual Rectangle Hurtbox {
        get {
            return new Rectangle(CollisionBoxX - 2, CollisionBoxY - 2,
                                 CollisionBox.Width + 4,
                                 CollisionBox.Height + 4);
        }
    } 
    public virtual Rectangle Hitbox {
        get {
            return new Rectangle(CollisionBoxX, CollisionBoxY - 2,
                                 CollisionBox.Width,
                                 CollisionBox.Height + 2);
        }
    }
    public int Damage;
    public int Health;
    public int MaxHealth;
    protected TimerManager _timerManager;
    protected DamageManager _damageManager;
    protected StateMachine<Character> _stateMachine;
    protected bool _invulnerable;
    protected bool _alive = true;
    protected Timer _invulnerableTimer;
    public HealthBar HealthBar;

    public void TakeDamage(int amount) {
        if (_invulnerable || !_alive) {
            Console.WriteLine("Invulnerable baby!");
            return;
        }
        Health -= amount;
        Console.WriteLine("About to change state to damaged");
        if (Health > 0) {
            _stateMachine.ChangeState("damaged");
        } else {
            _stateMachine.ChangeState("dying");
        }
        Console.WriteLine("Successfully changed state");
    }
    public void OnDamage() {
        _invulnerableTimer.Start();
        _invulnerable = true;
    }
    public void OnRecover() {
        _invulnerable = false;
    }
    public void OnDeath() {
        Console.WriteLine("OnDeath");
        _alive = false;
        Console.WriteLine("Change state to dying");
    }
    public void Respawn() {
        _stateMachine.ChangeState("respawning");
    }
    public void OnRespawn() {
        _alive = true;
        _stateMachine.ChangeState("idle");
    }
    public void DamageTimerAlarm() {
        Console.WriteLine("Damage timer alarm");
        _stateMachine.ChangeState("idle");
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        HealthBar.Draw(spriteBatch);
    }
}