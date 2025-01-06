using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Entities;
public abstract class Actor {
	public virtual Texture2D Texture { get; set; }

	// Positional Values
	public virtual Vector2 Position { get; set; }
	protected Vector2 _facingDirection;
	protected Vector2 _previousPosition;

	// Collision Values
	public virtual Rectangle CollisionBox { get; set; }
	protected float _collisionBoxSize;
	protected int _collisionBoxHeight;
	protected float _scale;
	
	// Movement Values
	protected int _speed;

	public abstract void Update();
	public abstract void Draw();

	// Collision Functions
	public virtual void Collide(Rectangle collided) {
		Position = _previousPosition;
		// If wall is to the left or right of player, allow vertical movement
		if (collided.Left >= Position.X + (Texture.Width * _scale) ||
			collided.Right <= Position.X) {
			Position = new Vector2(Position.X, CollisionBox.Y - (float) (Texture.Height * _scale) + _collisionBoxHeight);
		}
		// If wall is above or below player, allow horizontal movement
		else if (collided.Top >= (int) (Position.Y + (Texture.Height * _scale)) ||
				 collided.Bottom <= Position.Y + (float) (Texture.Height * _scale) - _collisionBoxHeight) {
			Position = new Vector2(CollisionBox.X, Position.Y);
		}
	}

	protected abstract void ProcessMovement();
}



