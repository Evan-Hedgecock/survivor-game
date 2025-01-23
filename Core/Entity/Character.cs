using Microsoft.Xna.Framework;
using Core.Objects;
using Core.Systems;
using System;
using Core.UI;
using Microsoft.Xna.Framework.Graphics;

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
    protected Timer _invulnerableTimer;
    public HealthBar HealthBar;

    public void TakeDamage(int amount) {
        if (_invulnerable) {
            Console.WriteLine("Invulnerable baby!");
            return;
        }
        Health -= amount;
        Console.WriteLine("About to change state to damaged");
        _stateMachine.ChangeState("damaged");
        Console.WriteLine("Successfully changed state");
    }
    public void OnDamage() {
        _invulnerableTimer.Start();
        _invulnerable = true;
    }
    public void OnRecover() {
        _invulnerable = false;
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