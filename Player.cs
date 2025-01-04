using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Character
{
    public class Player
    {
        private Texture2D Texture { get; set; }
        private Vector2 Position;
        private int Speed;

        public Player(Texture2D texture, Vector2 startPos, int speed)
        {
            Texture = texture;
            Position = startPos;
            Speed = speed;
        }

		public void Update(Vector2 inputAxis)
		{
			Position.X += (inputAxis.X * Speed);
			Position.Y += (inputAxis.Y * Speed);
		}

        public void Draw(SpriteBatch spriteBatch)
        {
			spriteBatch.Draw(Texture, Position, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
		}

		public Vector2 getPosition()
		{
			return Position;
		}
	}
}
