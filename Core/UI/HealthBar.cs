using System;
using Core.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.UI;

public class HealthBar(int height, int width, Character owner) : ProgressBar(height, width)
{
    protected Vector2 _offset = new(-1 * ((width / 2) - owner.Bounds.Width / 2), -2 - height);
    protected Character _owner = owner;
    protected float _percent = 1f;

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_backgroundTexture, _area, Color.Red);
        spriteBatch.Draw(_foregroundTexture, _foregroundArea, Color.Green);
    }

    public override void Update()
    {
        _percent = (float)_owner.Health / _owner.MaxHealth;
        string healthDetails = string.Format("Health: {0}\nMaxHealth: {1}\nPercent: {2}\n",
                                             _owner.Health, _owner.MaxHealth, _percent);
        Console.WriteLine(healthDetails);
        _area.X = (int)(_owner.BoundsX + _offset.X);
        _area.Y = (int)(_owner.BoundsY + _offset.Y);
        _foregroundArea = _area;
        _foregroundArea.Width = (int)(_area.Width * _percent);
    }
    public void LoadTextures(Texture2D foregroundTexture, Texture2D backgroundTexture)
    {
        _foregroundTexture = foregroundTexture;
        _backgroundTexture = backgroundTexture;
    }
}
