using System;
using Microsoft.Xna.Framework;
using Core;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Objects;
public abstract class GameObject
{
    public Rectangle Bounds {
        get { return new Rectangle((int)PositionX, (int)PositionY,
                                   _bounds.Width, _bounds.Height); }
        set { _bounds = value; } 
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
    }

    public virtual void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(Texture, Bounds, null, Color.White, Rotation,
                         new Vector2(0, 0), SpriteEffects.None, 0.0f);
    }
}