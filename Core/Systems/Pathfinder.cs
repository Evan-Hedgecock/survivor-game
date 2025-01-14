using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Core.Systems;

namespace Core.Systems;
public class Pathfinder {
	private GameGrid _gameGrid;
	private Node[,] _nodeGrid;

	public Pathfinder(GameGrid gameGrid) {
		_gameGrid = gameGrid;
		_nodeGrid = gameGrid.NodeGrid;
	}

	public List<Vector2> FindPath(Node start, Node target) {
		//Console.WriteLine(start.WorldPosition);
		//Console.WriteLine(target.WorldPosition);
		List<Node> openList = new List<Node>();
		List<Node> closedList = new List<Node>();

		// Set current node equal to start
		// Add current node to openList
		// Enter loop
			// Set node from openList with lowest fCost as current,
			// if tie, set node with lowest gCost
			// If current equals the target node, return
			// Remove current from openList
			// Add current to closedList
			// For every neighbor of current
				// If neighbor is in closed list or is blocked
				// continue to next neighbor
				// If neighbor is not in openlist, or new path to neighbor is shorter
					// Set fCost of neighbor
					// if neighbor is not in openList
					// Add neighbor to openList

		Node current = start;
		//Console.Write("Start node position: ");
		//Console.WriteLine(start.WorldPosition);
		openList.Add(current);
		while (openList.Count > 0) {
			//Console.WriteLine("Finding path");
			//Console.WriteLine(openList.Count);
			current = openList[0];
			foreach (Node node in openList) {
				if (node.fCost < current.fCost || 
				   (node.fCost == current.fCost &&
					node.gCost < current.gCost)) {
					current = node;
				}
			}

			if (current == target) {
				List<Vector2> waypoints = new List<Vector2>();
					//Console.WriteLine("Add waypoints");
				while (current != start) {
					//Console.WriteLine(current.WorldPosition);
					waypoints.Add(current.WorldPosition);
					current = current.Parent;
				}
				//Console.WriteLine(current.WorldPosition);
				waypoints.Add(current.WorldPosition);
				waypoints.Reverse();
				waypoints.RemoveAt(0);
				return waypoints;
			}

			openList.Remove(current);
			closedList.Add(current);

			for (int row = -1; row <= 1; row++) {
				for (int col = -1; col <= 1; col++) {
					if (row == 0 && col == 0) {
						continue;
					}
					Node neighbor;
					try {
						neighbor = _nodeGrid[current.Row + row,
												  current.Col + col];
					} catch (Exception) {
						continue;
					}
					if (closedList.Contains(neighbor) || neighbor.Blocked) {
						continue;
					}

					int newPathToNeighbor = current.gCost + CalculateMoveCost(current, neighbor);
					if (!openList.Contains(neighbor) ||
						newPathToNeighbor < neighbor.gCost) {
						neighbor.gCost = newPathToNeighbor;
						neighbor.hCost = CalculateMoveCost(neighbor, target);
						neighbor.Parent = current;
						if (!openList.Contains(neighbor)) {
							openList.Add(neighbor);
						}
					}
				}
			}
		}
		Console.WriteLine("Uh oh, the target was not found...");
		return new List<Vector2>();
	}


	public int CalculateMoveCost(Node start, Node target) {
		int rowDiff = Math.Abs(start.Row - target.Row);
		int colDiff = Math.Abs(start.Col - target.Col);

		if (rowDiff < colDiff) {
			return (14 * rowDiff) + (10 * (colDiff - rowDiff));
		} else {
			return (14 * colDiff) + (10 * (rowDiff - colDiff));
		}
	}
}





		
