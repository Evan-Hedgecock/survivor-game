using Microsoft.Xna.Framework;
using Core.Objects;
using Core.Physics;

namespace Core.Character;

public class Player(Rectangle bounds) : PhysicsObject(bounds) {

    public void Initialize() {
        Acceleration = 900;
        Friction = 900;
        Velocity = new Vector2(0, 0);
        MaxSpeed = 200;
        CollisionBoxHeight = Bounds.Height / 3;
        CollisionBoxY = CollisionBoxY + Bounds.Height - CollisionBox.Height;
        PositionX = BoundsX;
        PositionY = BoundsY;
        InitializeCollisionManager();
    }
    
    public void Update(Vector2 inputAxis, GameTime gameTime) {
        MoveAndSlide(inputAxis, gameTime);
    }
}
