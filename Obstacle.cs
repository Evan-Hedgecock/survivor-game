using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Collider;

namespace Obstacle;
public class Wall : StaticCollider {
	private Texture2D _texture { get; set; }
	private Vector2 _position;

	public Wall(Vector2 position) {
		_position = position;
	}

	public void SetTexture(Texture2D texture) {
		_texture = texture;
		CreateShape();
	}

	public void Draw(SpriteBatch spriteBatch) {
		spriteBatch.Draw(_texture, _collisionShape, Color.White);
	}

	private void CreateShape() {
		_collisionShape = new Rectangle((int) _position.X, (int) _position.Y, _texture.Width, _texture.Height);
	}
}
