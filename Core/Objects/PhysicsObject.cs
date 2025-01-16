using System;
using Microsoft.Xna.Framework;
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
    public int Deceleration { get; set; }
    public int MaxSpeed { get; set; }
    protected CollisionManager _collisionManager;

    public PhysicsObject(Rectangle bounds, CollisionManager collisionManager) : base(bounds) {
        _maxVelocity = new Vector2(MaxSpeed, MaxSpeed);
        _minVelocity = new Vector2(-1 * MaxSpeed, -1 * MaxSpeed);
        _collisionManager = collisionManager;
    }
    //private CollisionManager _collisionManager;
    public virtual void MoveAndCollide(Vector2 direction, GameTime gameTime) {
        // Move CollisionBox by velocity
        // Check collisons
        // If there is a collision
            // Get the normal vector2 of the wall that was collided with
            // Stop movement just before that vector2
        // If there is no collision
            // Move Position by velocity
            // Reset collision box
        
        Move(direction, gameTime);
        Console.Write("This: ");
        Console.WriteLine(this);
        if (!_collisionManager.IsColliding(this)) {
            Console.WriteLine("Not colliding");
        } else {
            Console.WriteLine("Colliding");
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
                VelocityX = (float)Math.Clamp(VelocityX - (Deceleration * deltaTime), 0, VelocityX);
            } else if (VelocityX < 0) {
                VelocityX = (float)Math.Clamp(VelocityX + (Deceleration * deltaTime), VelocityX, 0);
            }
        }
        if (direction.Y < 0) {
            VelocityY = (float)(VelocityY - (Acceleration * deltaTime));
        } else if (direction.Y > 0) {
            VelocityY = (float)(VelocityY + (Acceleration * deltaTime));
        } else {
            if (VelocityY > 0) {
                VelocityY = (float)Math.Clamp(VelocityY - (Deceleration * deltaTime), 0, VelocityY);
            } else if (VelocityY < 0) {
                VelocityY = (float)Math.Clamp(VelocityY + (Deceleration * deltaTime), VelocityY, 0);
            }
        }
        string movementValues = string.Format("Velocity: {0}\n Position: {1}\ndirection: {2}",
                                              Velocity, Position, direction);
        Console.WriteLine(movementValues);
        PositionX += (float)(VelocityX * deltaTime);
        PositionY += (float)(VelocityY * deltaTime);
    }
    public void Move(Vector2 direction, GameTime gameTime, Rectangle body) {
        string before= string.Format("Body before: {0}", body);
        Console.WriteLine(before);
        double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;
        if (direction.X < 0) {
            VelocityX = (float)(VelocityX - (Acceleration * deltaTime));
        } else if (direction.X > 0) {
            VelocityX = (float)(VelocityX + (Acceleration * deltaTime));
        } else {
            if (VelocityX > 0) {
                VelocityX = (float)Math.Round(VelocityX - (Deceleration * deltaTime));
            } else if (VelocityX < 0) {
                VelocityX = (float)Math.Round(VelocityX + (Deceleration * deltaTime));
            }
        }
        if (direction.Y < 0) {
            VelocityY = (float)(VelocityY - (Acceleration * deltaTime));
        } else if (direction.Y > 0) {
            VelocityY = (float)(VelocityY + (Acceleration * deltaTime));
        } else {
            if (VelocityY > 0) {
                VelocityY = (float)Math.Round(VelocityY - (Deceleration * deltaTime));
            } else if (VelocityY < 0) {
                VelocityY = (float)Math.Round(VelocityY + (Deceleration * deltaTime));
            }
        }
        body.X += (int)(VelocityX * deltaTime);
        body.Y += (int)(VelocityY * deltaTime);
        string after = string.Format("Body after: {0}", body);
        Console.WriteLine(after);
    }
}
