using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Core.Systems.World;

namespace Core.Utils;
public class Pathfinder {
	// Properties:
	// GameGrid, array of GridCells in world
	// Waypoints, array of GridCell.Center points
	// PathCells, array of GridCells with additional H and G values
	//
	// Methods:
	// FindPath(Point position) returns array of waypoints
		// Takes a position to find a path towards
		// Performs A* algorithm
		// Once optimal path is found,
		// add the center of each cell along path to waypoints array
		// return waypoints array
	
	public GridCell[,] Grid { get; set; }

	private GridCell _targetCell;
	private GridCell _startCell;

	public Pathfinder(GridCell[,] grid) {
		Grid = grid;
	}

	public List<Vector2> FindPath() {
		List<PathCell> openList = new List<PathCell>();
		List<PathCell> closedList = new List<PathCell>();
		PathCell current;

		openList.Add(new PathCell(_startCell));
		// Add start cell to openList
		// Infinite loop
			// set current to cell in openList with lowest Total cost
			// remove current from open and add to closed
			//
			// if current is the target node then return path
			//
			// foreach neighbor of current cell
				// if neighbor is not traversable or in closedList
					// continue
				// Create a path cell with current as parent
				// if new path to neighbor is shorter or neighbor is not in open
				// set / update neighbor f cost
				// set neighbor parent to current
				// if neighbor is not in open
					// add neighbor to open
		while(true) {
			current = openList[0];
			for (int i = 0; i < openList.Count; i++) {
				//Console.WriteLine("Checking for which cell in openList to set as current");
				if (openList[i].TotalCost < current.TotalCost) {
					current = openList[i];
				}
				else if (openList[i].TotalCost == current.TotalCost) {
					if (openList[i].EstimatedCost < current.EstimatedCost) {
						current = openList[i];
					}
				}
			}
			//Console.WriteLine("Remove current from openList and add to closedList");
			openList.Remove(current);
			closedList.Add(current);
			if (current.GridPosition[0] == _targetCell.GridPosition[0] &&
				current.GridPosition[1] == _targetCell.GridPosition[1]) {
				List<Vector2> waypoints = new List<Vector2>();
				PathCell currentPath = current;
				while (true) {
					try {
						waypoints.Add(currentPath.Position);
						currentPath = currentPath.Parent;
					}
					catch (Exception) {
						break;
					}
				}
				waypoints.Reverse();
				// Remove the starting position because it is where the entity
				// finding path is currently positioned. Probably.
				Console.WriteLine("Waypoints: ");
				foreach (Vector2 waypoint in waypoints) {
					Console.WriteLine(waypoint);
				}
				return waypoints;
			}

			Console.WriteLine("Before checking neighbors");
			for (int i = -1; i <= 1; i++) {
				for (int j = -1; j <= 1; j++) {
					if (i == 0 && j == 0) {
						continue;
					}
					// If neighbor is blocked, or neighbor is in closed list
						// Continue to next neighbor
					// If this new path to neighbor is shorter, or neighbor is not in open list
						// set fcost of neighbor
						// set parent of neighbor to current
						// add neighbor to open list
					GridCell neighbor;
					try {
						neighbor = Grid[current.GridPosition[0] + i,
										current.GridPosition[1] + j];
					} catch (Exception) {
						Console.WriteLine("Neighbor outside grid bounds");
						continue;
					}

					if (neighbor.Blocked) {
						Console.WriteLine("Neighbor is blocked");
						continue;
					}
					bool neighborInClosed = false;
					foreach (PathCell cell in closedList) {
						if (cell.GridPosition[0] == neighbor.GridPosition[0] &&
							cell.GridPosition[1] == neighbor.GridPosition[1]) {
							neighborInClosed = true;
							break;
						}
					}
					if (neighborInClosed) {
						continue;
					}
					PathCell path = new PathCell(neighbor, _targetCell);
					bool pathInOpen = false;
					foreach (PathCell cell in openList) {
						if (cell.GridPosition[0] == path.GridPosition[0] &&
							cell.GridPosition[1] == path.GridPosition[1]) {
							pathInOpen = true;
							break;
						}
					}
					if (!pathInOpen) {
						path.Parent = current;
						path.SetTotalCost();
						Console.WriteLine("adding path to openlist");
						openList.Add(path);
					}
				}
			}
		}
	}

	public bool FindTargetCell(Vector2 targetPosition) {
		foreach (GridCell cell in Grid) {
			if (cell.Cell.Contains(targetPosition)) {
				_targetCell = cell;
				return true;
			}
		}
		return false;
	}

	public bool FindStartCell(Vector2 startPosition) {
		foreach (GridCell cell in Grid) {
			if (cell.Cell.Contains(startPosition)) {
				_startCell = cell;
				return true;
			}
		}
		return false;
	}
}

public class PathCell : GridCell {
	
	public int ActualCost { get; set; }
	public int EstimatedCost { get; set; }
	public int TotalCost { get; set; }
	public PathCell Parent { get; set; }
	public GridCell Target { get; set; }

	// Used for the starting position cell
	public PathCell(GridCell gridCell) : base(gridCell.Position, gridCell.Size, gridCell.GridPosition) {
		TotalCost = 0;
	}

	public PathCell(GridCell gridCell, GridCell target) : base(gridCell.Position, gridCell.Size, gridCell.GridPosition) {
		GridPosition = new int[] {gridCell.GridPosition[0], gridCell.GridPosition[1]};
		Target = target;
	}

	public void SetTotalCost() {
		if (GridPosition[0] - Parent.GridPosition[0] == 0 ||
			GridPosition[1] - Parent.GridPosition[1] == 0) {
			ActualCost = 10;
		}
		else {
			ActualCost = 14;
		}

		EstimatedCost = 0;
		int targetRowDiff = Math.Abs(GridPosition[0] - Target.GridPosition[0]);
		int targetColDiff = Math.Abs(GridPosition[1] - Target.GridPosition[1]);
		// Increase EstimatedCost diagonally until straight line is available
		while (targetRowDiff > 0 && targetColDiff > 0) {
			EstimatedCost += 14;
			targetRowDiff -= 1;
			targetColDiff -= 1;
		}
		EstimatedCost += (10 * targetRowDiff) + (10 * targetColDiff);
		TotalCost = ActualCost + EstimatedCost;
	}
}
// 		Console.Write("Current cell position: [");
// 		Console.Write(current.GridPosition[0]);
// 		Console.Write(", ");
// 		Console.Write(current.GridPosition[1]);
// 		Console.WriteLine("]");
//
//		Console.WriteLine("Neighbors: ");
//		foreach (PathCell neighbor in openList) {
//			Console.Write(neighbor.GridPosition[0]);
//			Console.Write(", ");
//			Console.Write(neighbor.GridPosition[1]);
//			Console.WriteLine("]");
//		}

// 		Console.WriteLine("Path cell position");
// 		Console.Write(gridCell.GridPosition[0]);
// 		Console.Write(", ");
// 		Console.Write(gridCell.GridPosition[1]);
// 		Console.WriteLine("]");
//
// 		Console.WriteLine("Start cell position");
// 		Console.Write(parent.GridPosition[0]);
// 		Console.Write(", ");
// 		Console.Write(parent.GridPosition[1]);
// 		Console.WriteLine("]");
// 
// 		Console.WriteLine("Target cell position");
// 		Console.Write(target.GridPosition[0]);
// 		Console.Write(", ");
// 		Console.Write(target.GridPosition[1]);
// 		Console.WriteLine("]");


