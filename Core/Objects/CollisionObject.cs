using System;
using Microsoft.Xna.Framework;

namespace Core.Objects;

public class CollisionObject : GameObject
{
    public CollisionObject(PhysicsObject collider, GameObject collided) : base(collided.Bounds) {
        GetDistance(collider, collided);
        GetPenDepth(collider, collided);
        GetNormal(collided);
    }
    public float Distance { get; set; }
    public Vector2 Normal { get; set; }
    public float PenDepth { get; set; }
    private Side _side;

    // Get distance between collider collision side and collided collision side
    private void GetDistance(PhysicsObject collider, GameObject collided) {
        float collisionBoxTopBeforeMove = collider.Bounds.Top +
                                          collider.Bounds.Height -
                                          collider.CollisionBox.Height;
        float rightDistance = MathHelper.Distance(collided.CollisionBox.Right,
                                                  collider.Bounds.Left);
        float leftDistance = MathHelper.Distance(collided.CollisionBox.Left,
                                                 collider.Bounds.Right);
        float topDistance = MathHelper.Distance(collided.CollisionBox.Top,
                                                collider.Bounds.Bottom);
        float bottomDistance = MathHelper.Distance(collided.CollisionBox.Bottom,
                                                   collisionBoxTopBeforeMove);
        Distance = MathHelper.Min(Math.Min(rightDistance, leftDistance),
                                  Math.Min(topDistance, bottomDistance));
    }

    // Get collider penetration depth into collided object
    private void GetPenDepth(PhysicsObject collider, GameObject collided) {
        float collisionBoxTopBeforeMove = collider.Bounds.Top +
                                          collider.Bounds.Height -
                                          collider.CollisionBox.Height;
        float topPen = collided.CollisionBox.Top - collider.Bounds.Bottom;
        float bottomPen = collisionBoxTopBeforeMove - collided.CollisionBox.Bottom;
        float rightPen = collider.Bounds.Left - collided.CollisionBox.Right;
        float leftPen = collided.CollisionBox.Left - collider.Bounds.Right;

        PenDepth = Math.Max(Math.Max(topPen, bottomPen),
                            Math.Max(rightPen, leftPen));
        if (PenDepth == topPen) {
            _side = Side.Top;
        } else if (PenDepth == bottomPen) {
            _side = Side.Bottom;
        } else if (PenDepth == leftPen) {
            _side = Side.Left;
        } else {
            _side = Side.Right;
        }
    }

    // Get normal vector of wall collided into
    private void GetNormal(GameObject collided) {
        var directionVector = _side switch
        {
            Side.Right => new Vector2(collided.CollisionBox.Right -
                                      collided.CollisionBox.Right,
                                      collided.CollisionBox.Top -
                                      collided.CollisionBox.Bottom),
            Side.Left => new Vector2(collided.CollisionBox.Left -
                                     collided.CollisionBox.Left,
                                     collided.CollisionBox.Bottom -
                                     collided.CollisionBox.Top),
            Side.Top => new Vector2(collided.CollisionBox.Left -
                                    collided.CollisionBox.Right,
                                    collided.CollisionBox.Top -
                                    collided.CollisionBox.Top),
            Side.Bottom => new Vector2(collided.CollisionBox.Right -
                                       collided.CollisionBox.Left,
                                       collided.CollisionBox.Bottom -
                                       collided.CollisionBox.Bottom),
            _ => new Vector2(0, 0),
    };
        directionVector.Rotate((float)Math.PI / 2);
        Vector2 normalVector = new((float)Math.Round(directionVector.X), (float)Math.Round(directionVector.Y));
        normalVector.Normalize();
        Normal = normalVector;
    }
}
