using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
    public Grid CollisionGrid = new(worldHeight, worldWidth, cellSize);
    public List<GameObject>[,] ObjectGrid; 

    public void Initialize() {
        ObjectGrid = new List<GameObject>[CollisionGrid.Height / cellSize,
                                                                 CollisionGrid.Width / cellSize];
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
    public void Update() {
    }
}