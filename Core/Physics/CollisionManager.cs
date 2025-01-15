using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Core.Objects;
using Core.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Physics;

public class CollisionManager(List<GameObject> staticGameObjects) {
    public List<GameObject> StaticObjects = staticGameObjects;
    //public List<GameObject> DynamicObjects = dynamicGameObjects;
    public LinkedList<GameObject> UpdateQueue = new();

    private static readonly int worldHeight = 500;
    private static readonly int worldWidth = 500;

    private static readonly int cellSize = 100;
    public static readonly Grid CollisionGrid = new(worldHeight, worldWidth, cellSize);
    public Node[,] NodeGrid = CollisionGrid.NodeGrid;
    public static List<GameObject>[,] ObjectGrid = new List<GameObject>[CollisionGrid.Height / cellSize,
                                                                 CollisionGrid.Width / cellSize];
    public void Initialize() {
        foreach (GameObject obj in StaticObjects) {
            // Get the nodes that obj collisionBox intersects with
            // Insert into ObjectGrid at each of the nodes grid positions
            Node[] nodes = CollisionGrid.WorldRectToNodes(obj.Bounds);
            foreach (Node node in nodes) {
                try {
                    ObjectGrid[node.Row, node.Col].Add(obj);
                } catch (NullReferenceException) {
                    if (node != null) {
                        ObjectGrid[node.Row, node.Col] = [obj];
                    }
                }
            }
        }
    }

    public static bool IsColliding(GameObject collider) {
        Node[] colliderInNodes = CollisionGrid.WorldRectToNodes(collider.CollisionBox);
        // If none of the nodes collider is in, has an object in it
        // then there are no collisions and can return false
        foreach (Node node in colliderInNodes) {
            if (ObjectGrid[node.Row, node.Col].Count == 0) {
                return false;
            } 
            foreach (GameObject obj in ObjectGrid[node.Row, node.Col]) {
                if (obj.CollisionBox.Intersects(collider.CollisionBox) && obj != collider) {
                    return true;
                }
            }
        }
        return false;
    }

    // public CollisionObject GetCollision(GameObject collider) {
    //     Node[] colliderInNodes = CollisionGrid.WorldRectToNodes(collider.CollisionBox);

    // }
}