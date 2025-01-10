using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core.Objects;

namespace Core.Systems.World;
public class GameGrid {
	// Start generating grid in a rectangle
	// Need the world height, width, and top left corner position
	
	// Properties:
	// Array[GridCell] that is the grid
	// Point width, height that is size of grid
	// Point x, y of top left corner in world position
	
	public GridCell[,] Grid { get; private set; }

	private Texture2D _texture;

	public Vector2 _position; 


	private int _rows;
	private int _columns;
	private int _cellSize = 30;

	public GameGrid(Rectangle world) {
		_position = new Vector2(world.X, world.Y);
		_rows = world.Width / _cellSize;
		_columns = world.Height / _cellSize;
		Grid = new GridCell[_rows, _columns];
	}

	// Convert world coordinates to game grid
	public void CreateGrid(Wall[] obstacles) {
		for (int i = 0; i < _rows; i++) {
			for (int j = 0; j < _columns; j++) {
				Grid[i, j] = new GridCell(new Vector2(_position.X + (i * _cellSize),
													_position.Y + (j * _cellSize)),
										  _cellSize, new int[2] {i, j});
				foreach (Wall obstacle in obstacles) {
					if (Grid[i, j].Cell.Intersects(obstacle.CollisionShape)) {
						Grid[i, j].TraversalSpeed = 0;
					}
				}
			}
		}
	}
}


public class GridCell {

	public Texture2D Texture { get; set; }

	public int Size {
		get { return _size; }
		set { _size = value; }
	}
	private int _size;

	public Color Colour { get; set; }

	public int[] GridPosition {
		get { return _gridPosition; }
		set { _gridPosition = value; }
	}
	private int[] _gridPosition;

	public Rectangle Cell {
		get { return _cell; }
		private set { _cell = value; }
	}
	private Rectangle _cell;

	public virtual Vector2 Position {
		get { return _position; }
		private set { _position = value; }
	}
	private Vector2 _position;

	public bool Blocked {
		get { return TraversalSpeed == 0; }
	}

	public float TraversalSpeed {
		get { return _traversalSpeed; }
		set { _traversalSpeed = value; }
	}
	private float _traversalSpeed;
	
	public GridCell(Vector2 position, int size, int[] gridPosition) {
		GridPosition = gridPosition;
		_size = size;
		Position = position;
		Cell = new Rectangle((int) position.X, (int) position.Y, size, size);
		TraversalSpeed = 1;
		Colour = Color.Purple;
	}

	public void Draw(SpriteBatch spriteBatch, int num) {
		if (Colour != Color.Purple) {
			spriteBatch.Draw(Texture, Cell, Colour);
			return;
		}
		if (num % 2 == 0) {
			spriteBatch.Draw(Texture, Cell, Color.CornflowerBlue);
		} else {
			spriteBatch.Draw(Texture, Cell, Color.Black);
		}
	}

}
