using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Systems;
public class Node {
	public int GCost { get; set; }
	public int HCost { get; set; }
	public int FCost {
		get {
			return GCost + HCost;
		}
	}
	public Node Parent { get; set; }

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

	public void Draw(SpriteBatch spriteBatch, Color color) {
		spriteBatch.Draw(Texture, Cell, color);
	}
}