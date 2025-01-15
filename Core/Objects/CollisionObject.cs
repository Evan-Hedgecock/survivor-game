using Microsoft.Xna.Framework;

namespace Core.Objects;

public class CollisionObject(Rectangle bounds) : GameObject(bounds)
{
    public Vector2 Distance { get; set; }
    public Vector2 Normal { get; set; }
    public float PenDepth { get; set; }
}
