using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Core.Objects;

public class CollisionObject : GameObject
{
    public CollisionObject(PhysicsObject collider, GameObject collided) : base(collided.Bounds) {
        // Get distance between the collider left and collided right
        // collider right and collided left
        // collider top and collided bottom
        // collider bottom and collider top
        // if the smallest value is not on the inside of collided
            // assign the smallest value to Distance
        // or else assign that value as PenDepth
        GetDistance(collider, collided);
        GetPenDepth(collider, collided);
        GetNormal(collided);
        string collisionDetails = string.Format("Normal: {0}\n" +
                                                "Distance: {1}\n" +
                                                "PenDepth: {2}",
                                                Normal, Distance, PenDepth);
        Console.WriteLine(collisionDetails);
    }
    public float Distance { get; set; }
    public Vector2 Normal { get; set; }
    public float PenDepth { get; set; }
    private Side _side;

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
        // Check to make sure the collision isn't on a perfect corner
        Distance = MathHelper.Min(Math.Min(rightDistance, leftDistance),
                                  Math.Min(topDistance, bottomDistance));
    }

    private void GetPenDepth(PhysicsObject collider, GameObject collided) {
        float collisionBoxTopBeforeMove = collider.Bounds.Top +
                                          collider.Bounds.Height -
                                          collider.CollisionBox.Height;
        // Subtract the positive direction of collider or collided side from the other
        // Return the smallest value
        float topPen = collided.CollisionBox.Top - collider.Bounds.Bottom;
        float bottomPen = collisionBoxTopBeforeMove - collided.CollisionBox.Bottom;
        float rightPen = collider.Bounds.Left - collided.CollisionBox.Right;
        float leftPen = collided.CollisionBox.Left - collider.Bounds.Right;

        string penDetails = string.Format("topPen: {0}\nbottomPen: {1}\nrightPen: {2}\nleftPen: {3}",
                                          topPen, bottomPen, rightPen, leftPen);
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
        string vectorDetails = string.Format("directionVector: {0}\nnormalVector: {1}\n" +
                                             "Normal: {2}",
                                              directionVector, normalVector, Normal);
        //Console.WriteLine(vectorDetails);
    }
}
