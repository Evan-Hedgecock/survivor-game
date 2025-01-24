using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Core.UI;

public abstract class ProgressBar(int height, int width) : BaseUiElement(height, width)
{
    protected Rectangle _foregroundArea;
    protected Texture2D _backgroundTexture;
    protected Texture2D _foregroundTexture;
}