using System;
using System.Collections.Generic;
using Core.Entity;

namespace Core.Systems;

public class DamageManager(List<Character> characters)
{
    public List<Character> Characters = characters;
    private static readonly int worldHeight = 500;
    private static readonly int worldWidth = 500;
    private static readonly int cellSize = 100;
    public static readonly Grid DamageGrid = new(worldHeight, worldWidth, cellSize);
    public Node[,] NodeGrid = DamageGrid.NodeGrid;
    public List<Character>[,] CharactersInGrid = new List<Character>[
        (DamageGrid.Height / cellSize),
        (DamageGrid.Width / cellSize)
    ];

    public void Initialize()
    {
        // Create empty arrays in every index of Static and Dynamic grid objects
        for (int row = 0; row < CharactersInGrid.GetLength(0); row++)
        {
            for (int col = 0; col < CharactersInGrid.GetLength(1); col++)
            {
                CharactersInGrid[row, col] = [];
            }
        }
        // Find all nodes that have a static object in them
        // Add those objects to every node they intersect with
        CreateCharacterList(CharactersInGrid, Characters);
    }
    public void Update()
    {
        UpdateCharacterList();
    }
    private static void CreateCharacterList(List<Character>[,] list, List<Character> characters)
    {
        foreach (Character character in characters)
        {
            Node[] nodes = DamageGrid.WorldRectToNodes(character.Bounds);
            foreach (Node node in nodes)
            {
                list[node.Row, node.Col].Add(character);
            }
        }
    }
    private void ClearCharacterList()
    {
        foreach (List<Character> list in CharactersInGrid)
        {
            list.Clear();
        }
    }
    private void UpdateCharacterList()
    {
        ClearCharacterList();
        CreateCharacterList(CharactersInGrid, Characters);
    }
    public bool IsDamaging(Character attacker)
    {
        // Find which node[s] the collider is in
        Node[] hitboxInNodes = DamageGrid.WorldRectToNodes(attacker.Hitbox);
        // For every node the collider is in
        // Check if there is a collision with any objects in static or dynamic list
        foreach (Node node in hitboxInNodes)
        {
            if (DealDamage(CharactersInGrid, node, attacker))
            {
                return true;
            }
        }
        return false;
    }
    private static bool DealDamage(List<Character>[,] characters, Node node, Character attacker)
    {
        if (characters[node.Row, node.Col].Count == 0)
        {
            return false;
        }
        // Check if it's intersecting with an object in the corresponding CollisionGridObjects array
        foreach (Character character in characters[node.Row, node.Col])
        {
            if (character.Hurtbox.Intersects(attacker.Hitbox) && character != attacker)
            {
                character.TakeDamage(attacker.Damage);
                return true;
            }
        }
        return false;
    }

}