using System;
using Microsoft.Xna.Framework;
using Core;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Objects;
public abstract class GameObject(Rectangle bounds)
{
    public Rectangle Bounds { get; set; } = bounds;
    public Vector2 Position { get; set; } = new Vector2(bounds.X, bounds.Y);
    public Rectangle CollisionBox { 
        get { return _collisionBox; }
        set { _collisionBox = value; } 
    }
    public int CollisionBoxHeight {
        set { _collisionBox.Height = value; }
    }
    protected Rectangle _collisionBox = new(bounds.X, bounds.Y,
                      (int)(bounds.Height * 0.2f), bounds.Width);
                                            
    public Texture2D Texture { get; set; }
    public Layer RenderLayer { get; set; }
    public Layer CollisionLayer { get; set; }
    public float Depth { get; set; }
    public float Scale { get; set; }
    public float Rotation { get; set; }

    public abstract void Update();
    public virtual void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(Texture, Bounds, null, Color.White, Rotation,
                         new Vector2(0, 0), SpriteEffects.None, 0.0f);
    }
}