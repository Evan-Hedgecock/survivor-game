using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core.Objects;

namespace Core.Objects;
public class Wall : StaticObject {

	public Wall(Vector2 position, int width, int height) {
		Position = position;
		CollisionShape = new Rectangle((int) Position.X, (int) Position.Y,  width, height);
	}

	public void CreateTexture(Texture2D texture) {
		Texture = texture;
	}

	public void Draw(SpriteBatch spriteBatch) {
		spriteBatch.Draw(Texture, CollisionShape, Color.White);
	}

	private void CreateShape() {
		CollisionShape = new Rectangle((int) Position.X, (int) Position.Y, (int) (Texture.Width * 0.2), Texture.Height);
	}
}
