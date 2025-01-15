using Microsoft.Xna.Framework;
using Core.Objects;
using System;
using System.ComponentModel;

namespace Core.Character;

public class Player(Rectangle bounds) : PhysicsObject(bounds) {

    public void Initialize() {
        Acceleration = 750;
        Deceleration = 600;
        Velocity = new Vector2(0, 0);
        MaxSpeed = 200;
    }
    
    public void Update(Vector2 inputAxis, GameTime gameTime) {
        Vector2 oldPos = Position;
        double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;
        if (inputAxis.X < 0) {
            VelocityX = (float)(VelocityX - (Acceleration * deltaTime));
        } else if (inputAxis.X > 0) {
            VelocityX = (float)(VelocityX + (Acceleration * deltaTime));
        } else {
            if (VelocityX > 0) {
                VelocityX = (float)Math.Round(VelocityX - (Deceleration * deltaTime));
            } else if (VelocityX < 0) {
                VelocityX = (float)Math.Round(VelocityX + (Deceleration * deltaTime));
            }
        }
        if (inputAxis.Y < 0) {
            VelocityY = (float)(VelocityY - (Acceleration * deltaTime));
        } else if (inputAxis.Y > 0) {
            VelocityY = (float)(VelocityY + (Acceleration * deltaTime));
        } else {
            if (VelocityY > 0) {
                VelocityY = (float)Math.Round(VelocityY - (Deceleration * deltaTime));
            } else if (VelocityY < 0) {
                VelocityY = (float)Math.Round(VelocityY + (Deceleration * deltaTime));
            }
        }
        PositionX += (float)(VelocityX * deltaTime);
        PositionY += (float)(VelocityY * deltaTime);
        string posDiff = string.Format("Old pos: {0}\nNew pos: {1}" + 
                                       "deltaTime: {2}\nVelocity: {3} Acceleration: {4}",
                                       oldPos, Position, deltaTime, Velocity, Acceleration);
        Console.WriteLine(posDiff);
    }
}
