using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Collider;

namespace Obstacle;
public class Wall : StaticCollider {
	private Texture2D _texture { get; set; }

	public Wall(Rectangle shape) {
		_collisionShape = shape;
	}

	public void SetTexture(Texture2D texture) {
		_texture = texture;
	}

	public void Draw(SpriteBatch spriteBatch) {
		spriteBatch.Draw(_texture, _collisionShape, Color.White);
	}
}
