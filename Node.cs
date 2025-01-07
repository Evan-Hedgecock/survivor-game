using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Grid;

public class Node {

	public int ActualCost;
	public int EstimatedCost;
	public int TotalCost;
	public Vector2 Position;

	public Texture2D Texture {
		get { return _texture; }
		set { _texture = value; }
	}

	private Texture2D _texture;

	public Node(Vector2 position) {
		Position = position;
	}

	public void Draw(SpriteBatch spriteBatch, int num) {
		var color = (num % 2 == 0) ? Color.White : Color.Black;
		spriteBatch.Draw(_texture, Position, null, color, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);
	}
}

