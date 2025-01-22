using Microsoft.Xna.Framework;
using Core.Objects;
using Core.Systems;

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
    protected DamageManager _damageManager;

    public void TakeDamage(int amount) {
        Health -= amount;
    }
}