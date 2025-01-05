using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Time;

namespace Character
{
    public class Player
    {
        private Texture2D texture { get; set; }

		// Positional values
        private Vector2 position = new Vector2(200, 200);
		private Vector2 facingDirection = new Vector2(1, 0);

		// Movement values
		private int dashSpeed = 10;
		private int speed = 5;

		// Ability bools
		private bool canDash = true;
		private bool dashing = false;

		// Durations
		private float dashDuration = 0.1F;

		// Cooldowns
		private float dashCooldown = 0.75F;

		public void Update(Vector2 inputAxis)
		{
			processMovement(inputAxis);

			// Change player direction on direction input
			if (inputAxis.X != 0 || inputAxis.Y != 0)
				facingDirection = inputAxis;
		}

        public void Draw(SpriteBatch spriteBatch)
        {
			spriteBatch.Draw(texture, position, null, Color.White, 0f, new Vector2(0, 0), 0.15f, SpriteEffects.None, 0f);
		}


		private void processMovement(Vector2 inputAxis)
		{
			position.X += (inputAxis.X * speed);
			position.Y += (inputAxis.Y * speed);
		}

        public void dash(Vector2 direction)
		{
			if (canDash || dashing)
			{
				// During first dash set dashing to true
				Console.WriteLine(dashing);
				Console.WriteLine(canDash);
				Console.WriteLine();
				if (canDash)
					dashing = true;
				canDash = false;
				if (direction.X == 0  && direction.Y == 0)
				{
					direction = facingDirection;
				}

				position.X += (direction.X * dashSpeed);
				position.Y += (direction.Y * dashSpeed);
			}
		}
		// Timer Functions
		public Timer dashCooldownTimer()
		{
			Action cb = dashReady;
			Timer timer = new Timer(dashCooldown, cb);
			return timer;
		}

		public Timer dashDurationTimer()
		{
			Action cb = dashComplete;
			Timer timer = new Timer(dashDuration, cb);
			return timer;
		}

		// Set ability bool functions
		private void dashReady()
		{
			Console.WriteLine("Dash ready");
			canDash = true;
		}
		
		private void dashComplete()
		{
			Console.WriteLine("Dash Complete");
			dashing = false;
		}

		// Getters
		public bool getDash()
		{
			return (canDash || dashing);
		}

		public Vector2 getPosition()
		{
			return position;
		}

		// Setters
		public void setTexture(Texture2D texture)
		{
			this.texture = texture;
		}
	}
}
