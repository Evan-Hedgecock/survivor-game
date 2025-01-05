using Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Weapon
{
	public class Gun
	{
		private Texture2D Texture { get; set; }
		private Vector2 Position;
		private Player User;
		
		public void Update(Vector2 position)
		{
			Position.X = position.X + 20;
			Position.Y = position.Y + 20;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Texture, Position, null, Color.White, 0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0f);
		}

		// Setters
		public void setTexture(Texture2D texture)
		{
			this.Texture = texture;
		}
	}
}
