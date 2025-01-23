using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Core.UI;

public abstract class BaseUiElement(int height, int width) {
    protected Rectangle _area = new(0, 0, width, height);
    protected bool _visible;

    public abstract void Update();
    public abstract void Draw(SpriteBatch spriteBatch);
}