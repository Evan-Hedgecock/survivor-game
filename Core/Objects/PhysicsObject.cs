using System;
using Microsoft.Xna.Framework;
using Core;
using Core.Physics;

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
    public int Friction { get; set; }
    public int MaxSpeed { get; set; }
    protected CollisionManager _collisionManager;

    public PhysicsObject(Rectangle bounds) : base(bounds) {
        _maxVelocity = new Vector2(MaxSpeed, MaxSpeed);
        _minVelocity = new Vector2(-1 * MaxSpeed, -1 * MaxSpeed);
        _collisionManager = Global.Services.GetService(typeof(CollisionManager)) as CollisionManager;
    }

    public void Initialize() {

    }
    public virtual void MoveAndSlide(Vector2 direction, GameTime gameTime) {
        Move(direction, gameTime);
        if (_collisionManager.IsColliding(this)) {
            var collisions = _collisionManager.GetCollision(this);
            foreach (CollisionObject collision in collisions)
            {
                if (collision.PenDepth < 0) {
                    BoundsX -= (int)(collision.PenDepth * collision.Normal.X);
                    BoundsY -= (int)(collision.PenDepth * collision.Normal.Y);
                    CollisionBoxX = PositionX;
                    CollisionBoxY = PositionY + Bounds.Height - CollisionBox.Height;
                }
            }
        }
    }

    public void Move(Vector2 direction, GameTime gameTime) {
        double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;
        if (direction.X < 0) {
            VelocityX = (float)(VelocityX - (Acceleration * deltaTime));
        } else if (direction.X > 0) {
            VelocityX = (float)(VelocityX + (Acceleration * deltaTime));
        } else {
            if (VelocityX > 0) {
                VelocityX = (float)Math.Clamp(VelocityX - (Friction * deltaTime), 0, VelocityX);
            } else if (VelocityX < 0) {
                VelocityX = (float)Math.Clamp(VelocityX + (Friction * deltaTime), VelocityX, 0);
            }
        }
        if (direction.Y < 0) {
            VelocityY = (float)(VelocityY - (Acceleration * deltaTime));
        } else if (direction.Y > 0) {
            VelocityY = (float)(VelocityY + (Acceleration * deltaTime));
        } else {
            if (VelocityY > 0) {
                VelocityY = (float)Math.Clamp(VelocityY - (Friction * deltaTime), 0, VelocityY);
            } else if (VelocityY < 0) {
                VelocityY = (float)Math.Clamp(VelocityY + (Friction * deltaTime), VelocityY, 0);
            }
        }
        string movementValues = string.Format("Velocity: {0}\n Position: {1}\ndirection: {2}",
                                              Velocity, Position, direction);
        BoundsX += (int)(VelocityX * deltaTime);
        BoundsY += (int)(VelocityY * deltaTime);
        CollisionBoxX = PositionX;
        CollisionBoxY = PositionY + Bounds.Height - CollisionBox.Height;
    }
    public Rectangle Move(Vector2 direction, GameTime gameTime, Rectangle oldRect) {
        string before= string.Format("Body before: {0}", oldRect);
        double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;
        if (direction.X < 0) {
            VelocityX = (float)(VelocityX - (Acceleration * deltaTime));
        } else if (direction.X > 0) {
            VelocityX = (float)(VelocityX + (Acceleration * deltaTime));
        } else {
            if (VelocityX > 0) {
                VelocityX = (float)Math.Clamp(VelocityX - (Friction * deltaTime), 0, VelocityX);
            } else if (VelocityX < 0) {
                VelocityX = (float)Math.Clamp(VelocityX + (Friction * deltaTime), VelocityX, 0);
            }
        }
        if (direction.Y < 0) {
            VelocityY = (float)(VelocityY - (Acceleration * deltaTime));
        } else if (direction.Y > 0) {
            VelocityY = (float)(VelocityY + (Acceleration * deltaTime));
        } else {
            if (VelocityY > 0) {
                VelocityY = (float)Math.Clamp(VelocityY - (Friction * deltaTime), 0, VelocityY);
            } else if (VelocityY < 0) {
                VelocityY = (float)Math.Clamp(VelocityY + (Friction * deltaTime), VelocityY, 0);
            }
        }
        string movementValues = string.Format("Velocity: {0}\n Position: {1}\ndirection: {2}",
                                              Velocity, Position, direction);
        Rectangle newRect = new Rectangle((int)(oldRect.X + (VelocityX * deltaTime)),
                             (int)(oldRect.Y + (VelocityY * deltaTime)),
                             oldRect.Width, oldRect.Height);
        return newRect;
    }
}
