using Core.Systems;
using Core.Entity;
using System.Drawing;

namespace Core.Items;

public abstract class Weapon(Character owner) : Equippable(owner) {
    protected int _damage;
    protected float _attackDuration;
    protected Timer _attackDurationTimer;
    protected float _attackCooldown;
    protected Timer _attackCooldownTimer;
    protected float _chargeTime;
    protected Timer _chargeTimer;
    protected bool _attackReady;
    public virtual Rectangle Hitbox
    {
        get
        {
            return _hitbox;
        }
        set
        {
            _hitbox = value;
        }
    }
    public int HitboxX
    {
        get
        {
            return _hitbox.X;
        }
        set
        {
            _hitbox.X = value;
        }
    }
    public int HitboxY
    {
        get
        {
            return _hitbox.Y;
        }
        set
        {
            _hitbox.Y = value;
        }
    }
    protected Rectangle _hitbox;
    public abstract void ChargeAttack();
    public abstract void PerformAttack();
    public abstract void UpdateAttack();
    protected abstract void AttackDurationAlarm();
    protected abstract void AttackCooldownAlarm();
    protected abstract void ChargeAlarm();
}