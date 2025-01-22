using System;
using Microsoft.Xna.Framework;

namespace Core.Systems;
public class Grid {

	public int Height { get; set; }
	public int Width { get; set; }
	public Rectangle Area { get; set; }
	public Vector2 GridSize { get; set; }
	public int CellSize { get; set; }

	public Node[,] NodeGrid { get; set; }

	public Grid(int height, int width, int cellSize) {
		// Create the Grid area centered around 0, 0
		int xPos = (width / 2) - width;
		int yPos = (height / 2) - height;
		Height = height;
		Width = width;
		Area = new Rectangle(xPos, yPos, width, height);
		CellSize = cellSize;
		NodeGrid = CreateGrid();
		GridSize = new Vector2(Width / CellSize, Height / CellSize);
	}

	private Node[,] CreateGrid() {
		int rowCount = Height / CellSize;
		int colCount = Width / CellSize;
		Node[,] grid = new Node[rowCount, colCount];

		for (int row = 0; row < rowCount; row++) {
			for (int col = 0; col < colCount; col++) {
				float xPos = Area.X + (CellSize * col);
				float yPos = Area.Y + (CellSize * row);
				Vector2 worldPos = new(xPos, yPos);
				grid[row, col] = new Node(row, col, CellSize, worldPos);
			}
		}
		return grid;
	}

	public Node WorldPosToNode(Vector2 worldPos) {
		// Find position percent across entire world area
		float percentX = (worldPos.X + Width / 2) / Width;
		float percentY = (worldPos.Y + Height / 2) / Height;
		percentX = Math.Clamp(percentX, 0, 1);
		percentY = Math.Clamp(percentY, 0, 1);
		// Conver the position percent to the percent of grid rows and columns
		int GridRow = Math.Clamp((int) (GridSize.Y * percentY), 0,
								 (int) GridSize.Y - 1); 
		int GridCol = Math.Clamp((int) (GridSize.X * percentX), 0,
								 (int) GridSize.X - 1); 
		return NearestUnblockedNode(NodeGrid[GridRow, GridCol]);
	}

	public Node NearestUnblockedNode(Node node) {
		Node current = node;
		int increment = 1;
		while (current.Blocked) {
			current = NodeGrid[node.Row - increment, node.Col];
			if (!current.Blocked) {
				break;
			}
			current = NodeGrid[node.Row + increment, node.Col];
			if (!current.Blocked) {
				break;
			}
			current = NodeGrid[node.Row, node.Col - increment];
			if (!current.Blocked) {
				break;
			}
			current = NodeGrid[node.Row, node.Col + increment];
			if (!current.Blocked) {
				break;
			}
			current = NodeGrid[node.Row - increment, node.Col - increment];
			if (!current.Blocked) {
				break;
			}
			current = NodeGrid[node.Row + increment, node.Col + increment];
			if (!current.Blocked) {
				break;
			}
			current = NodeGrid[node.Row - increment, node.Col + increment];
			if (!current.Blocked) {
				break;
			}
			current = NodeGrid[node.Row + increment, node.Col - increment];
			if (!current.Blocked) {
				break;
			}
			increment++;
		}
		return current;
	}

	public Node[] WorldRectToNodes(Rectangle worldRect) {
		// Find the topLeft and bottomRight Nodes according to their world position
		Node topLeft = WorldPosToNode(new Vector2(worldRect.X, worldRect.Y));
		Node bottomRight = WorldPosToNode(new Vector2(worldRect.Right, worldRect.Bottom));
		int startRow = topLeft.Row;
		int startCol = topLeft.Col;
		int endRow = bottomRight.Row;
		int endCol = bottomRight.Col;
		Node[] nodes = new Node[(endRow - startRow + 1) *
								(endCol - startCol + 1)];
		int nodesIndex = 0;
		// Loop over every node in between the topLeft and bottomRight
		// Adding to a return array
		for (int row = 0; row < (endRow - startRow + 1); row++) {
			for (int col = 0; col < (endCol - startCol + 1); col++) {
				nodes[nodesIndex] = NodeGrid[row + startRow, col + startCol];
				nodesIndex++;
			}
		}
		return nodes;
	}
}