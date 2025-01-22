using Microsoft.Xna.Framework;
using Core.Objects;
using System;
using Core.Physics;

namespace Core.Character;

public class Player(Rectangle bounds, CollisionManager collisionManager) :
             PhysicsObject(bounds, collisionManager) {

    public void Initialize() {
        Acceleration = 900;
        Deceleration = 900;
        Velocity = new Vector2(0, 0);
        MaxSpeed = 200;
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
        MoveAndSlide(inputAxis, gameTime);
        string values = string.Format("Bounds: {0}\nCollisionBox: {1}\n" +
                                      "Position: {2}\n", Bounds, CollisionBox, Position);
        Console.WriteLine(values);
    }
}
