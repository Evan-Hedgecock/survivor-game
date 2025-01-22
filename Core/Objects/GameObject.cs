using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Objects;
public abstract class GameObject
{
    public Rectangle Bounds {
        get { return _bounds; }
        set { _bounds = value; }
    }
    public int BoundsX {
        get { return Bounds.X; }
        set { _bounds.X = value; }
    }
    public int BoundsY {
        get { return Bounds.Y; }
        set { _bounds.Y = value; }
    }
    protected Rectangle _bounds;
    public Vector2 Position { 
        get { return new Vector2(_bounds.X, _bounds.Y); } 
    }
    public int PositionX {
        get { return _bounds.X; }
        set { _bounds.X = value; }
    }
    public int PositionY {
        get { return _bounds.Y; }
        set { _bounds.Y = value; }
    }
    public Rectangle CollisionBox { 
        get { return _collisionBox; }
        set { _collisionBox = value; } 
    }
    public int CollisionBoxX {
        get { return _collisionBox.X; }
        set { _collisionBox.X = value; }
    }
    public int CollisionBoxY {
        get { return _collisionBox.Y; }
        set { _collisionBox.Y = value; }
    }
    public int CollisionBoxHeight {
        set { _collisionBox.Height = value; }
    }
    protected Rectangle _collisionBox;
                                            
    public Texture2D Texture { get; set; }
    public Layer RenderLayer { get; set; }
    public Layer CollisionLayer { get; set; }
    public float Depth { get; set; }
    public float Scale { get; set; }
    public float Rotation { get; set; }

    public GameObject(Rectangle bounds) {
        Bounds = bounds;
        CollisionBox = new Rectangle(bounds.X, bounds.Y,
                                     bounds.Width, bounds.Height);
    }

    public virtual void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(Texture, Bounds, null, Color.White, Rotation,
                         new Vector2(0, 0), SpriteEffects.None, 0.0f);
    }
}