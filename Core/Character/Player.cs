using Microsoft.Xna.Framework;
using Core.Objects;
using System;
using Core.Physics;

namespace Core.Character;

public class Player(Rectangle bounds, CollisionManager collisionManager) :
             PhysicsObject(bounds, collisionManager) {

    public void Initialize() {
        Acceleration = 750;
        Deceleration = 600;
        Velocity = new Vector2(0, 0);
        MaxSpeed = 200;
    }
    
    public void Update(Vector2 inputAxis, GameTime gameTime) {
        MoveAndCollide(inputAxis, gameTime);
        CollisionBoxX = (int)PositionX;
        CollisionBoxY = (int)PositionY;
        BoundsX = (int)PositionX;
        BoundsY = (int)PositionY;
        string values = string.Format("Bounds: {0}\nCollisionBox: {1}\n" +
                                      "Position: {2}\n", Bounds, CollisionBox, Position);
        Console.WriteLine(values);
    }
}
