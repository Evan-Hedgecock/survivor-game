using System;
using Microsoft.Xna.Framework;
using Core.Entity;

namespace Core.Items;

public class Sword(Character owner) : Weapon(owner)
{
    public override void Initialize()
    {
        _attackReady = true;
        _attackCooldown = 0.1f;
        _attackCooldownTimer = new(_attackCooldown, AttackCooldownAlarm);
        _attackDuration = 0.2f;
        _attackDurationTimer = new(_attackDuration, AttackDurationAlarm);
        _chargeTime = 0.05f;
        _chargeTimer = new(_chargeTime, ChargeAlarm);
        _equipPosition = _owner.WeaponPosition;
        _equipOffset = new(0, 0);
        _drawPosition = _equipPosition;
        _scale = 0.15f;
        _rotation = 0f;

    }

    public override void Update()
    {
        _equipPosition = _owner.WeaponPosition;
        _drawPosition = Vector2.Add(_equipPosition, _equipOffset);
        Console.Write("Updating sword _equipPosition to: ");
        Console.WriteLine(_equipPosition);
    }

    public override void ChargeAttack()
    {
        throw new System.NotImplementedException();
    }

    public override void PerformAttack()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateAttack()
    {
        throw new System.NotImplementedException();
    }

    protected override void AttackCooldownAlarm()
    {
        throw new System.NotImplementedException();
    }

    protected override void AttackDurationAlarm()
    {
        throw new System.NotImplementedException();
    }

    protected override void ChargeAlarm()
    {
        throw new System.NotImplementedException();
    }
}