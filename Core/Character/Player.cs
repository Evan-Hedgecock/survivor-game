using Microsoft.Xna.Framework;
using Core.Objects;
using System;
using Core.Physics;

namespace Core.Character;

public class Player(Rectangle bounds, CollisionManager collisionManager) :
             PhysicsObject(bounds, collisionManager) {

    public void Initialize() {
        Acceleration = 300;
        Deceleration = 400;
        Velocity = new Vector2(0, 0);
        MaxSpeed = 500;
        CollisionBoxHeight = Bounds.Height / 3;
        CollisionBoxY = CollisionBoxY + Bounds.Height - CollisionBox.Height;
        PositionX = BoundsX;
        PositionY = BoundsY;
        Console.Write("Bounds: ");
        Console.WriteLine(Bounds);
        Console.Write("Position: ");
        Console.WriteLine(Position);

    }
    
    public void Update(Vector2 inputAxis, GameTime gameTime) {
        Console.Write("Before updates Position: ");
        Console.WriteLine(Position);
        MoveAndCollide(inputAxis, gameTime);
        CollisionBoxX = (int)PositionX;
        CollisionBoxY = (int)PositionY + Bounds.Height - CollisionBox.Height;
        BoundsX = (int)PositionX;
        BoundsY = (int)PositionY;
        string values = string.Format("Bounds: {0}\nCollisionBox: {1}\n" +
                                      "Position: {2}\n", Bounds, CollisionBox, Position);
        Console.WriteLine(values);
    }
}
