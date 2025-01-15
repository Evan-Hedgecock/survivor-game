using System;
using System.Net;
using Microsoft.Xna.Framework;
using static Core.Physics.CollisionManager;
namespace Core.Objects;

public class PhysicsObject : GameObject
{
    public Vector2 Velocity { 
        get {
            return _velocity;
        } 
        set {
            _velocity = Vector2.Clamp(value, _minVelocity, _maxVelocity);
        }
    }
    public float VelocityX {
        get { return _velocity.X; }
        set { _velocity.X = Math.Clamp(value, -1 * MaxSpeed, MaxSpeed); }
    }
    public float VelocityY {
        get { return _velocity.Y; }
        set { _velocity.Y = Math.Clamp(value, -1 * MaxSpeed, MaxSpeed); }
    }
    protected Vector2 _velocity;
    protected Vector2 _maxVelocity;
    protected Vector2 _minVelocity;
    public int Acceleration { get; set; }
    public int Deceleration { get; set; }
    public int MaxSpeed { get; set; }

    public PhysicsObject(Rectangle bounds) : base(bounds) {
        _maxVelocity = new Vector2(MaxSpeed, MaxSpeed);
        _minVelocity = new Vector2(-1 * MaxSpeed, -1 * MaxSpeed);
    }
    //private CollisionManager _collisionManager;
    public virtual void MoveAndCollide() {
        // Move CollisionBox by velocity
        // Check collisons
        // If there is a collision
            // Get the normal vector2 of the wall that was collided with
            // Stop movement just before that vector2
        // If there is no collision
            // Move Position by velocity
            // Reset collision box
        _collisionBox.X += (int)Velocity.X;
        _collisionBox.Y += (int)Velocity.Y;
        if (!IsColliding(this)) {
            _position.X += (int)Velocity.X;
            _position.Y += (int)Velocity.Y;
        }
    }
}
