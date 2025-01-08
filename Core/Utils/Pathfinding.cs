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
	
	public bool FindTargetCell(Vector2 targetPosition) {
		foreach (GridCell cell in Grid) {
			if (cell.Cell.Contains(targetPosition)) {
				_targetCell = cell;
//				Console.Write("Target cell in grid position: ");
//				Console.Write(_targetCell.GridPosition[0]);
//				Console.Write(", ");
//				Console.Write(_targetCell.GridPosition[1]);
//				Console.WriteLine();
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

	public void FindPath() {
		// Set _startCell to current
		// Calculate ActualCost of all current neighbors
		List<PathCell> openList = new List<PathCell>();
		List<PathCell> closedList = new List<PathCell>();

		GridCell current = _startCell;

		for (int i = -1; i <= 1; i++) {
			for (int j = -1; j <= 1; j++) {
				try {
					if (i == 0 && j == 0) {
						continue;
					}
					PathCell neighbor = new PathCell(
											Grid[current.GridPosition[0] + i,
												 current.GridPosition[1] + j],
											_startCell, _targetCell);
					openList.Add(neighbor);
				}
				catch (Exception) {
					Console.WriteLine("Trying to create a neighbor cell that is outside of grid");
				}
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
	}
}

public class PathCell : GridCell {
	
	public int ActualCost { get; set; }
	public int EstimatedCost { get; set; }
	public int TotalCost { get; set; }

	public PathCell(GridCell gridCell, GridCell start, GridCell target) : base(gridCell.Position, gridCell.Size, gridCell.GridPosition) {
 		Console.WriteLine("Path cell position");
 		Console.Write(gridCell.GridPosition[0]);
 		Console.Write(", ");
 		Console.Write(gridCell.GridPosition[1]);
 		Console.WriteLine("]");

 		Console.WriteLine("Start cell position");
 		Console.Write(start.GridPosition[0]);
 		Console.Write(", ");
 		Console.Write(start.GridPosition[1]);
 		Console.WriteLine("]");
 
 		Console.WriteLine("Target cell position");
 		Console.Write(target.GridPosition[0]);
 		Console.Write(", ");
 		Console.Write(target.GridPosition[1]);
 		Console.WriteLine("]");

		int row = gridCell.GridPosition[0];
		int col = gridCell.GridPosition[1];

		if (row - start.GridPosition[0] == 0 ||
			col - start.GridPosition[1] == 0) {
			ActualCost = 10;
		}
		else {
			ActualCost = 14;
		}

		EstimatedCost = 0;
		int targetRowDiff = Math.Abs(row - target.GridPosition[0]);
		int targetColDiff = Math.Abs(col - target.GridPosition[1]);
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
