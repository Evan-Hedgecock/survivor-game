using System.Collections.Generic;
using Core.Objects;
using Core.Systems;

namespace Core.Physics;

public class CollisionManager(List<GameObject> staticObjects)
{
    public List<GameObject> StaticObjects = staticObjects;
    public LinkedList<GameObject> UpdateQueue = new();

    private static readonly int worldHeight = 500;
    private static readonly int worldWidth = 500;

    private static readonly int cellSize = 100;
    public static readonly Grid CollisionGrid = new(worldHeight, worldWidth, cellSize);
    public Node[,] NodeGrid = CollisionGrid.NodeGrid;

    // This 2D array of lists correlates to the NodeGrid
    // Each node in NodeGrid has its own list of GameObjects
    public List<GameObject>[,] CollisionGridObjects = new List<GameObject>[
                                                          (CollisionGrid.Height / cellSize),
                                                          (CollisionGrid.Width / cellSize)
                                                        ];

    public void Initialize() {
        // Create empty arrays for every node in CollisionGrid to store objects in that node
        for (int row = 0; row < CollisionGridObjects.GetLength(0); row++) {
            for (int col = 0; col < CollisionGridObjects.GetLength(1); col++) {
                CollisionGridObjects[row, col] = [];
            }
        }
        // Find all nodes that have a static object in them
        // Add those objects to every node they intersect with
        foreach (GameObject obj in StaticObjects) {
            Node[] nodes = CollisionGrid.WorldRectToNodes(obj.Bounds);
            foreach (Node node in nodes) {
                CollisionGridObjects[node.Row, node.Col].Add(obj);
            }
        }
    }

    public bool IsColliding(GameObject collider) {
        // Find which node[s] the collider is in
        Node[] colliderInNodes = CollisionGrid.WorldRectToNodes(collider.CollisionBox);
        // For every node the collider is in
        foreach (Node node in colliderInNodes) {
            if (CollisionGridObjects[node.Row, node.Col].Count == 0) {
                continue;
            } 
            // Check if it's intersecting with an object in the corresponding CollisionGridObjects array
            foreach (GameObject obj in CollisionGridObjects[node.Row, node.Col]) {
                if (obj.CollisionBox.Intersects(collider.CollisionBox) && obj != collider) {
                    return true;
                }
            }
        }
        return false;
    }

    public List<CollisionObject> GetCollision(PhysicsObject collider) {
        // Find which nodes the collider is in
        Node[] colliderInNodes = CollisionGrid.WorldRectToNodes(collider.CollisionBox);
        List<CollisionObject> collisionList = [];
        List<GameObject> collided = [];
        // For each of those nodes, if the corresponding CollisionGridObjects array is not empty
        foreach (Node node in colliderInNodes) {
            if (CollisionGridObjects[node.Row, node.Col].Count == 0) {
                continue;
            } 
            // Then check if each object interects with the collider
            foreach (GameObject obj in CollisionGridObjects[node.Row, node.Col]) {
                // If it does intersect, create a collisionObject corresponding to that collision
                // And add the object to a collided list to ensure no object is added twice
                if (obj.CollisionBox.Intersects(collider.CollisionBox) &&
                    obj != collider && !collided.Contains(obj)) {
                    collisionList.Add(new(collider, obj));
                    collided.Add(obj);
                }
            }
        }
        return collisionList;
    }
}