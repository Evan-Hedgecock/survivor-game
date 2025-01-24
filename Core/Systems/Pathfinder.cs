using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Core.Systems;
public class Pathfinder(Grid gameGrid)
{
	private readonly Node[,] _nodeGrid = gameGrid.NodeGrid;

	public List<Vector2> FindPath(Node start, Node target)
	{
		List<Node> openList = [];
		List<Node> closedList = [];

		Node current = start;
		openList.Add(current);
		while (openList.Count > 0)
		{
			current = openList[0];
			foreach (Node node in openList)
			{
				if (node.FCost < current.FCost ||
				   (node.FCost == current.FCost &&
					node.GCost < current.GCost))
				{
					current = node;
				}
			}

			if (current == target)
			{
				List<Vector2> waypoints = [];
				while (current != start)
				{
					waypoints.Add(current.WorldPosition);
					current = current.Parent;
				}
				waypoints.Add(current.WorldPosition);
				waypoints.Reverse();
				// waypoints.RemoveAt(0);
				return waypoints;
			}

			openList.Remove(current);
			closedList.Add(current);

			for (int row = -1; row <= 1; row++)
			{
				for (int col = -1; col <= 1; col++)
				{
					if (row == 0 && col == 0)
					{
						continue;
					}
					Node neighbor;
					try
					{
						neighbor = _nodeGrid[current.Row + row,
												  current.Col + col];
					}
					catch (Exception)
					{
						continue;
					}
					if (closedList.Contains(neighbor) || neighbor.Blocked)
					{
						continue;
					}

					int newPathToNeighbor = current.GCost + CalculateMoveCost(current, neighbor);
					if (!openList.Contains(neighbor) ||
						newPathToNeighbor < neighbor.GCost)
					{
						neighbor.GCost = newPathToNeighbor;
						neighbor.HCost = CalculateMoveCost(neighbor, target);
						neighbor.Parent = current;
						if (!openList.Contains(neighbor))
						{
							openList.Add(neighbor);
						}
					}
				}
			}
		}
		Console.WriteLine("Uh oh, the target was not found...");
		return [];
	}


	public static int CalculateMoveCost(Node start, Node target)
	{
		int rowDiff = Math.Abs(start.Row - target.Row);
		int colDiff = Math.Abs(start.Col - target.Col);

		if (rowDiff < colDiff)
		{
			return (14 * rowDiff) + (10 * (colDiff - rowDiff));
		}
		else
		{
			return (14 * colDiff) + (10 * (rowDiff - colDiff));
		}
	}
}






