using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core.Entity;

namespace Core.Items;

public abstract class Equippable(Character owner) {
    public Texture2D Texture;
    protected Character _owner = owner;
    protected bool _equipped = false;
    protected Vector2 _equipPosition;
    protected Vector2 _equipOffset;
    protected Vector2 _drawPosition;
    protected float _scale;
    protected float _rotation;
    protected SpriteEffects _spriteEffects = SpriteEffects.None;
    public virtual void Unequip() {
        _equipped = false;
    }
    public virtual void Equip() {
        _equipped = true;
    }
    public abstract void Initialize();
    public abstract void Update();
    public virtual void Draw(SpriteBatch spriteBatch) {
        if (_equipped) {
            Console.Write("Drawing equippable at position: ");
            Console.WriteLine(_drawPosition);
            spriteBatch.Draw(Texture, _drawPosition, null, Color.White, _rotation,
                             new Vector2(0, 0), _scale, _spriteEffects, 0f);
        }
    }
}