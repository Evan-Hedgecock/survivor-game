using System;
using Microsoft.Xna.Framework;
using Core;
using Microsoft.VisualBasic.FileIO;
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
        get { return _position; } 
        set { _position = Position; }
    }
    public float PositionX {
        get { return _position.X; }
        set { _position.X = value; }
    }
    public float PositionY {
        get { return _position.Y; }
        set { _position.Y = value; }
    }
    protected Vector2 _position;
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
                                      bounds.Width, (int)(bounds.Height * 0.5));
        Position = new Vector2(bounds.X, bounds.Y);
        Console.WriteLine(Position);
    }

    public virtual void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(Texture, Bounds, null, Color.White, Rotation,
                         new Vector2(0, 0), SpriteEffects.None, 0.0f);
    }
}