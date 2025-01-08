using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Entities;
public abstract class Actor {
	public virtual Texture2D Texture { 
		get { return _texture; }
		set { _texture = value; }
	}
	protected Texture2D _texture;
	protected float _scale;

	// Positional Values
	public virtual Vector2 Position {
		get { return _position; }
		set { _position = value; }
	}

	protected Vector2 _position;
	protected Vector2 _facingDirection;
	protected Vector2 _previousPosition;

	public Vector2 Center { get; set; }

	// Collision Values
	public virtual Rectangle CollisionBox {
		get { 
			UpdateCollider();
			return _collisionBox; }
		set { _collisionBox = value; }
	}
	protected Rectangle _collisionBox;
	protected float _collisionBoxOffset;
	protected int _collisionBoxHeight;
	
	// Movement Values
	public virtual int Speed { 
		get { return _speed; }
	   	set { _speed = value; }
	}
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

	public virtual void CreateTexture(Texture2D texture) {
		Texture = texture;
		CreateCollisionBox();
		UpdateCenter();
	}

	protected void UpdateCenter() {
		Center = new Vector2(_position.X + _texture.Width / 2, 
							 _position.Y + _texture.Height / 2);
	}

	protected virtual void CreateCollisionBox() {
		_collisionBoxOffset = ((_texture.Height * _scale) - _collisionBoxHeight);
		_collisionBox = new Rectangle((int) _position.X,
									  (int) (_position.Y + _collisionBoxOffset),
									  (int) (_texture.Width * _scale),
									  _collisionBoxHeight);
	}

	protected virtual void UpdateCollider() {
		_collisionBox.X = (int) _position.X;
		_collisionBox.Y = (int) (_position.Y + _collisionBoxOffset);
	}

	protected abstract void ProcessMovement(Vector2 direction);
}



