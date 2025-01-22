using Microsoft.Xna.Framework;
using Core.Systems;

namespace Core.Entity;

public class Player(Rectangle bounds) : Character(bounds) {

    public void Initialize() {
        // Movement properties
        Acceleration = 900;
        Friction = 900;
        Velocity = new Vector2(0, 0);
        MaxSpeed = 200;

        // Positional properties
        CollisionBoxHeight = Bounds.Height / 3;
        CollisionBoxY = CollisionBoxY + Bounds.Height - CollisionBox.Height;
        PositionX = BoundsX;
        PositionY = BoundsY;

        // Health and damage properties
        Health = 200;
        Damage = 20;

        // Services
        _damageManager = Global.Services.GetService(typeof(DamageManager)) as DamageManager;
        InitializeCollisionManager();
    }
    
    public void Update(Vector2 inputAxis, GameTime gameTime) {
        MoveAndSlide(inputAxis, gameTime);
    }
}
