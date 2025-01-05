using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Character
{
    public class Player
    {
        private Texture2D texture { get; set; }
        private Vector2 position;
		private Vector2 facingDirection;

		private int dashSpeed;
        private int speed;

        public Player(Texture2D texture)
        {
            this.texture = texture;
            position = new Vector2(200, 200);
            speed = 5;
			dashSpeed = 10;
        }

		public void Update(Vector2 inputAxis)
		{
			position.X += (inputAxis.X * speed);
			position.Y += (inputAxis.Y * speed);
			if (inputAxis.X != 0 || inputAxis.Y != 0)
				facingDirection = inputAxis;
		}

        public void Draw(SpriteBatch spriteBatch)
        {
			spriteBatch.Draw(texture, position, null, Color.White, 0f, new Vector2(0, 0), 0.15f, SpriteEffects.None, 0f);
		}

		public Vector2 getPosition()
		{
			return position;
		}

        public void dash(Vector2 direction)
		{
			if (direction.X == 0  && direction.Y == 0)
			{
				direction = facingDirection;
			}

			position.X += (direction.X * dashSpeed);
			position.Y += (direction.Y * dashSpeed);
		}
				
	}
}
