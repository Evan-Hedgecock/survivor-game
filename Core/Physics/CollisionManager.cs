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

public class CollisionManager {
    public List<GameObject> StaticObjects;
    //public List<GameObject> DynamicObjects = dynamicGameObjects;
    public LinkedList<GameObject> UpdateQueue = new();

    private static readonly int worldHeight = 500;
    private static readonly int worldWidth = 500;

    private static readonly int cellSize = 100;
    public static readonly Grid CollisionGrid = new(worldHeight, worldWidth, cellSize);
    public Node[,] NodeGrid = CollisionGrid.NodeGrid;
    public List<GameObject>[,] ObjectGrid;
    public CollisionManager(List<GameObject> staticObjects) {
        StaticObjects = staticObjects;
        ObjectGrid = new List<GameObject>[(CollisionGrid.Height / cellSize),
                                          (CollisionGrid.Width / cellSize)];
    }
    public void Initialize() {
        for (int row = 0; row < ObjectGrid.GetLength(0); row++) {
            for (int col = 0; col < ObjectGrid.GetLength(1); col++) {
                ObjectGrid[row, col] = [];
            }
        }
        foreach (GameObject obj in StaticObjects) {
            // Get the nodes that obj collisionBox intersects with
            // Insert into ObjectGrid at each of the nodes grid positions
            Node[] nodes = CollisionGrid.WorldRectToNodes(obj.Bounds);
            foreach (Node node in nodes) {
                ObjectGrid[node.Row, node.Col].Add(obj);
            }
        }
    }

    public bool IsColliding(GameObject collider) {
        Console.WriteLine("Start of IsColliding");
        Node[] colliderInNodes = CollisionGrid.WorldRectToNodes(collider.CollisionBox);
        // If none of the nodes collider is in, has an object in it
        // then there are no collisions and can return false
        foreach (Node node in colliderInNodes) {
            if (ObjectGrid[node.Row, node.Col].Count == 0) {
                continue;
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