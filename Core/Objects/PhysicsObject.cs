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
    }


    public virtual void MoveAndSlide(Vector2 direction, GameTime gameTime) {
        // Move this in direction
        Move(direction, gameTime);
        // If it is then colliding, get all of the collisions details
        if (_collisionManager.IsColliding(this)) {
            var collisions = _collisionManager.GetCollision(this);
            // For every collision, move this away from collision wall by penDepth
            foreach (CollisionObject collision in collisions)
            {
                if (collision.PenDepth < 0) {
                    BoundsX -= (int)(collision.PenDepth * collision.Normal.X);
                    BoundsY -= (int)(collision.PenDepth * collision.Normal.Y);
                    UpdateCollisionBox();
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
        string movementValues = string.Format("Velocity: {0}\n Position: {1}\ndirection: {2}\nDeltatime: {3}\n" +
                                              "\nXMovement: {4}\nYMovement: {5}\n",
                                              Velocity, Position, direction, deltaTime,
                                              VelocityX * deltaTime, VelocityY * deltaTime);
        Console.WriteLine(movementValues);
        BoundsX += (int)Math.Round(VelocityX * deltaTime);
        BoundsY += (int)Math.Round(VelocityY * deltaTime);
        UpdateCollisionBox();
    }
    protected void InitializeCollisionManager() {
        _collisionManager = Global.Services.GetService(typeof(CollisionManager)) as CollisionManager;
    }
    private void UpdateCollisionBox() {
        CollisionBoxX = PositionX;
        CollisionBoxY = PositionY + Bounds.Height - CollisionBox.Height;
    }
}
