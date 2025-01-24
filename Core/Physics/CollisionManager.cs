using System;
using System.Collections.Generic;
using Core.Objects;
using Core.Systems;
using Microsoft.Xna.Framework;

namespace Core.Physics;

public class CollisionManager(List<GameObject> staticObjects, List<GameObject> dynamicObjects)
{
    public List<GameObject> StaticObjects = staticObjects;
    public List<GameObject> DynamicObjects = dynamicObjects;
    public LinkedList<GameObject> UpdateQueue = new();

    private static readonly int worldHeight = 500;
    private static readonly int worldWidth = 500;

    private static readonly int cellSize = 100;
    public static readonly Grid CollisionGrid = new(worldHeight, worldWidth, cellSize);
    public Node[,] NodeGrid = CollisionGrid.NodeGrid;

    // This 2D array of lists correlates to the NodeGrid
    // Each node in NodeGrid has its own list of GameObjects
    public List<GameObject>[,] StaticGridObjects = new List<GameObject>[
                                                          (CollisionGrid.Height / cellSize),
                                                          (CollisionGrid.Width / cellSize)
                                                        ];
    public List<GameObject>[,] DynamicGridObjects = new List<GameObject>[
                                                          (CollisionGrid.Height / cellSize),
                                                          (CollisionGrid.Width / cellSize)
                                                        ];

    public void Initialize()
    {
        // Create empty arrays in every index of Static and Dynamic grid objects
        for (int row = 0; row < StaticGridObjects.GetLength(0); row++)
        {
            for (int col = 0; col < StaticGridObjects.GetLength(1); col++)
            {
                StaticGridObjects[row, col] = [];
                DynamicGridObjects[row, col] = [];
            }
        }
        // Find all nodes that have a static object in them
        // Add those objects to every node they intersect with
        CreateObjectsList(StaticGridObjects, StaticObjects);
    }

    public void Update()
    {
        UpdateDynamicObjects();
    }

    private static void CreateObjectsList(List<GameObject>[,] list, List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            Node[] nodes = CollisionGrid.WorldRectToNodes(obj.Bounds);
            foreach (Node node in nodes)
            {
                list[node.Row, node.Col].Add(obj);
            }
        }
    }

    private void ClearDynamicObjects()
    {
        foreach (List<GameObject> list in DynamicGridObjects)
        {
            list.Clear();
        }
    }

    private void UpdateDynamicObjects()
    {
        ClearDynamicObjects();
        CreateObjectsList(DynamicGridObjects, DynamicObjects);
    }

    public bool IsColliding(PhysicsObject collider)
    {
        // Find which node[s] the collider is in
        Node[] colliderInNodes = CollisionGrid.WorldRectToNodes(collider.CollisionBox);
        // For every node the collider is in
        // Check if there is a collision with any objects in static or dynamic list
        foreach (Node node in colliderInNodes)
        {
            if (CheckObjectListCollisions(StaticGridObjects, node, collider) ||
                CheckObjectListCollisions(DynamicGridObjects, node, collider))
            {
                return true;
            }
        }
        return false;
    }
    public List<CollisionObject> GetCollision(PhysicsObject collider)
    {
        // Find which nodes the collider is in
        Node[] colliderInNodes = CollisionGrid.WorldRectToNodes(collider.CollisionBox);
        List<CollisionObject> collisionList = [];
        List<GameObject> collided = [];
        foreach (Node node in colliderInNodes)
        {
            GetCollisionFromList(DynamicGridObjects, node, collider, collisionList, collided);
            GetCollisionFromList(StaticGridObjects, node, collider, collisionList, collided);
        }
        return collisionList;
    }
    private static void GetCollisionFromList(List<GameObject>[,] list, Node node,
                                             PhysicsObject collider,
                                             List<CollisionObject> collisionList,
                                             List<GameObject> collided)
    {
        // if the list at node array is not empty
        if (list[node.Row, node.Col].Count == 0)
        {
            return;
        }
        // Then check if each object interects with the collider
        foreach (GameObject obj in list[node.Row, node.Col])
        {
            // If it does intersect, create a collisionObject corresponding to that collision
            // And add the object to a collided list to ensure no object is added twice
            if (obj.CollisionBox.Intersects(collider.CollisionBox) &&
                obj != collider && !collided.Contains(obj))
            {
                collisionList.Add(new(collider, obj));
                collided.Add(obj);
            }
        }
    }
    private static bool CheckObjectListCollisions(List<GameObject>[,] objects, Node node, GameObject collider)
    {
        if (objects[node.Row, node.Col].Count == 0)
        {
            return false;
        }
        // Check if it's intersecting with an object in the corresponding CollisionGridObjects array
        foreach (GameObject obj in objects[node.Row, node.Col])
        {
            if (obj.CollisionBox.Intersects(collider.CollisionBox) && obj != collider)
            {
                return true;
            }
        }
        return false;
    }
}