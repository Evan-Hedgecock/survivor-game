using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Entities;
public abstract class Actor {
	public virtual Texture2D Texture { get; set; }

	// Positional Values
	public virtual Vector2 Position {
		get { return _position; }
		set { _position = value; }
	}

	protected Vector2 _position;
	protected Vector2 _facingDirection;
	protected Vector2 _previousPosition;

	// Collision Values
	public virtual Rectangle CollisionBox {
		get { return _collisionBox; }
		set { _collisionBox = value; }
	}
//
	protected Rectangle _collisionBox;
	protected float _collisionBoxOffset;
	protected int _collisionBoxHeight;
	protected float _scale;
	
	// Movement Values
	public virtual int Speed { get; set; }
	protected int _speed;

	public abstract void Draw(SpriteBatch spriteBatch);

	// Collision Functions
	public virtual void Collide(Rectangle collided) {
		_position = _previousPosition;
		// If wall is to the left or right of player, allow vertical movement
		if (collided.Left >= _position.X + (Texture.Width * _scale) ||
			collided.Right <= _position.X) {
			_position.Y = _collisionBox.Y - (float) (Texture.Height * _scale) + _collisionBoxHeight;
		}
		// If wall is above or below player, allow horizontal movement
		else if (collided.Top >= (int) (_position.Y + (Texture.Height * _scale)) ||
				 collided.Bottom <= _position.Y + (float) (Texture.Height * _scale) - _collisionBoxHeight) {
			_position.X = _collisionBox.X;
		}
	}

	protected abstract void ProcessMovement(Vector2 direction);
}



