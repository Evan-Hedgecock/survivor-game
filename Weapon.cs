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
		
		public Gun(Texture2D texture, Player player)
		{
			Texture = texture;
			User = player;
		}

		public void Update(Vector2 position)
		{
			Position.X = position.X + 70;
			Position.Y = position.Y + 40;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Texture, Position, Color.White);
		}
	}
}
