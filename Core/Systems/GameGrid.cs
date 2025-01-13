using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Systems;
public class GameGrid {

	public int Height { get; set; }
	public int Width { get; set; }
	public Rectangle Area { get; set; }
	public Vector2 GridSize { get; set; }

	public Node[,] NodeGrid { get; set; }

	public GameGrid(int height, int width) {
		int xPos = (width / 2) - width;
		int yPos = (height / 2) - height;
		Height = height;
		Width = width;
		// Create the GameGrid area centered around 0, 0
		Area = new Rectangle(xPos, yPos, width, height);
		int nodeSize = 20;
		NodeGrid = CreateGrid(nodeSize);
		GridSize = new Vector2(Width / nodeSize, Height / nodeSize);
	}

	private Node[,] CreateGrid(int nodeSize) {
		int rowCount = Height / nodeSize;
		int colCount = Width / nodeSize;
		Node[,] grid = new Node[rowCount, colCount];

		for (int row = 0; row < rowCount; row++) {
			for (int col = 0; col < colCount; col++) {
				float xPos = Area.X + (nodeSize * col);
				float yPos = Area.Y + (nodeSize * row);
				Vector2 worldPos = new Vector2(xPos, yPos);
				grid[row, col] = new Node(row, col, nodeSize, worldPos);
			}
		}
		return grid;
	}

	public Node WorldPosToNode(Vector2 worldPos) {
		float percentX = (worldPos.X + Width / 2) / Width;
		float percentY = (worldPos.Y + Height / 2) / Height;
		percentX = Math.Clamp(percentX, 0, 1);
		percentY = Math.Clamp(percentY, 0, 1);
		int GridRow = Math.Clamp((int) (GridSize.Y * percentY), 0,
								 (int) GridSize.Y - 1); 
		int GridCol = Math.Clamp((int) (GridSize.X * percentX), 0,
								 (int) GridSize.X - 1); 
		return NodeGrid[GridRow, GridCol];
	}

	public Node[] WorldRectToNodes(Rectangle worldRect) {
		// Calculate starting node in top left corner
		// And end node in bottom right corner
		Node topLeft = WorldPosToNode(new Vector2(worldRect.X, worldRect.Y));
		Node bottomRight = WorldPosToNode(new Vector2(worldRect.Right, worldRect.Bottom));
		int startRow = topLeft.Row;
		int startCol = topLeft.Col;
		int endRow = bottomRight.Row;
		int endCol = bottomRight.Col;
		Node[] nodes = new Node[(endRow - startRow + 1) *
								(endCol - startCol + 1)];
		string startToEnd = string.Format("Start row, col: [{0}, {1}]\n" +
										  "End row, col: [{2}, {3}]\n",
										  startRow, startCol,
										  endRow, endCol);
		// Add all nodes in between start and end to nodes[]
		for (int row = 0; row < (endRow - startRow + 1); row++) {
			for (int col = 0; col < (endCol - startCol + 1); col++) {
				nodes[row + col] = NodeGrid[row + startRow, col + startCol];
			}
		}
		return nodes;
	}
}

public class Node {
	public int gCost { get; set; }
	public int hCost { get; set; }
	public int fCost {
		get {
			return gCost + hCost;
		}
	}

	public bool Blocked { get; set; }

	public int Size { get; set; }
	public int Row { get; set; }
	public int Col { get; set; }
	public Vector2 WorldPosition { get; set; }
	public Rectangle Cell { get; set; }

	public Texture2D Texture { get; set; }

	public Node(int row, int col, int size, Vector2 worldPos) {
		Row = row;
		Col = col;
		Size = size;
		WorldPosition = worldPos;
		Cell = new Rectangle((int) WorldPosition.X, (int) WorldPosition.Y, Size, Size);
		// Initialize blocked to false, this changes in the Initialize() if it contains a wall or other obstacle
		Blocked = false;
	}

	public void Draw(SpriteBatch spriteBatch, Node playerPos) {
		if (Cell == playerPos.Cell) {
			spriteBatch.Draw(Texture, Cell, Color.Blue);
		} else if ((Row + Col) % 2 == 0) {
			spriteBatch.Draw(Texture, Cell, Color.Gray);
		} else {
			spriteBatch.Draw(Texture, Cell, Color.Black);
		}
	}

	public void Draw(SpriteBatch spriteBatch) {
		if ((Row + Col) % 2 == 0) {
			spriteBatch.Draw(Texture, Cell, Color.Gray);
		} else {
			spriteBatch.Draw(Texture, Cell, Color.Black);
		}
	}
}
	


