using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Obstacle;
using Time;

namespace Character
{
    public class Player
    {
        private Texture2D texture { get; set; }

		// Positional values
        private Vector2 position = new Vector2(200, 200);
		private Vector2 facingDirection = new Vector2(1, 0);
		private Vector2 previousPosition = new Vector2(200, 200);
		private Rectangle collisionBox;

		// Movement values
		private int dashSpeed = 15;
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
			ProcessMovement(inputAxis);

			// Change player direction on direction input
			if (inputAxis.X != 0 || inputAxis.Y != 0)
				facingDirection = inputAxis;
		}

        public void Draw(SpriteBatch spriteBatch)
        {
			spriteBatch.Draw(texture, position, null, Color.White, 0f, new Vector2(0, 0), 0.15f, SpriteEffects.None, 0f);
		}


		private void ProcessMovement(Vector2 inputAxis)
		{
			previousPosition = position;
			if (dashing)
				Dash(facingDirection);
			else
			{
				position.X += (inputAxis.X * speed);
				position.Y += (inputAxis.Y * speed);
			}
		}

        public void Dash(Vector2 direction)
		{
			if (canDash || dashing)
			{
				// During first dash set dashing to true
				if (canDash)
					dashing = true;
				canDash = false;
				if (direction.X == 0  && direction.Y == 0)
				{
					direction = facingDirection;
				}
				float dSpeed = (direction.X != 0 && direction.Y != 0) ? (float) (dashSpeed / 1.5) : dashSpeed;
				position.X += (direction.X * dSpeed);
				position.Y += (direction.Y * dSpeed);
			}
		}

		// Collision Functions
		public void Collide(Rectangle collider)
		{
			position = previousPosition;
			// If wall is to the left or right of player, allow vertical movement
			if (collider.Left >= position.X + (texture.Width * 0.15) ||
			    collider.Right <= position.X)
				position.Y = collisionBox.Y - (float) (texture.Height * 0.15) + 10;
			// If wall is above or below player, allow horizontal movement
			else if (collider.Top >= (int) (position.Y + (texture.Height * 0.15)) ||
				collider.Bottom <= position.Y + (float) (texture.Height * 0.15) - 10)
				position.X = collisionBox.X;
			else 
			{
				Console.Write("Collider.Top: ");
				Console.Write(collider.Top);
				Console.WriteLine();
				Console.Write("position.Y + height: ");
				Console.Write((int) (position.Y + (texture.Height * 0.15)));
				Console.WriteLine();
			}

			
		}

		// Timer Functions
		public Timer DashCooldownTimer()
		{
			Action cb = DashReady;
			Timer timer = new Timer(dashCooldown, cb);
			return timer;
		}

		public Timer DashDurationTimer()
		{
			Action cb = DashComplete;
			Timer timer = new Timer(dashDuration, cb);
			return timer;
		}

		// Set ability bool functions
		private void DashReady()
		{
			canDash = true;
		}
		
		private void DashComplete()
		{
			dashing = false;
		}

		// Getters
		public bool GetDash()
		{
			return (canDash || dashing);
		}

		public Vector2 GetPosition()
		{
			return position;
		}

		public Rectangle GetCollider()
		{
			collisionBox.X = (int) position.X;
			collisionBox.Y = (int) (position.Y + (this.texture.Height * 0.15f)) - 10;
			return collisionBox;
		}

		// Setters
		public void SetTexture(Texture2D texture)
		{
			this.texture = texture;
			collisionBox = new Rectangle(
					(int) this.position.X,
					(int) (this.position.Y + (this.texture.Height * 0.15f)) - 10,
				    (int) (this.texture.Width * 0.15f), 10);
		}
	}
}
