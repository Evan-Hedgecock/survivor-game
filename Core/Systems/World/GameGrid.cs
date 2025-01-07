using System;
using Microsoft.Xna.Framework;

namespace Core.Systems.World;
public class GameGrid {
	// Start generating grid in a rectangle
	// Need the world height, width, and top left corner position
	
	// Properties:
	// Array[GridCell] that is the grid
	// Point width, height that is size of grid
	// Point x, y of top left corner in world position
	
	public GridCell[,] Grid { get; private set; }


	public Point _position; 

	private int _rows;
	private int _columns;
	private int _cellSize = 10;

	public GameGrid(Rectangle world) {
		_position = new Point(world.X, world.Y);
		_rows = world.Width / _cellSize;
		_columns = world.Height / _cellSize;
		Grid = new GridCell[_rows, _columns];
	}

	// Convert world coordinates to game grid
	public void CreateGrid() {
		for (int i = 0; i < _rows; i++) {
			for (int j = 0; j < _columns; j++) {
				Grid[i, j] = new GridCell(new Point(_position.X + (i * _cellSize),
													_position.Y + (j * _cellSize)),
										  _cellSize);
			}
		}
	}
}


// Convert game grid to world coordinates
public class GridCell {

	private int _size;

	public Point Position {
		get { return _position; }
		private set { _position = value; }
	}
	private Point _position;

	public bool Blocked {
		get { return TraversalSpeed == 0; }
	}

	public float TraversalSpeed {
		get { return _traversalSpeed; }
		set { _traversalSpeed = value; }
	}
	private float _traversalSpeed;
	
	public GridCell(Point position, int size, float traversalSpeed = 1) {
		_size = size;
		TraversalSpeed = traversalSpeed;
		Position = position;
	}
}
