using Collider;
using Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace Obstacle
{
	public class Wall : StaticCollider
	{
		private Texture2D Texture { get; set; }

		public Wall(Rectangle shape)
		{
			this.CollisionShape = shape;
		}

		public void SetTexture(Texture2D texture)
		{
			this.Texture = texture;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Texture, CollisionShape, Color.White);
		}

		public void Update(Player player)
		{
			while (CheckCollision(player.GetCollider()))
				player.Collide();
		}

		private bool CheckCollision(Rectangle collider)
		{
			return CollisionShape.Intersects(collider);
		}

		private bool CheckContaining(Rectangle collider)
		{
			return CollisionShape.Contains(collider);
		}
	}
}

