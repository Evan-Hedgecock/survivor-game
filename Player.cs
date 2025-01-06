using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Time;

namespace Character;
public class Player {
	private Texture2D _texture { get; set; }

	// Positional values
	private Vector2 _position = new Vector2(200, 200);
	private Vector2 _facingDirection = new Vector2(1, 0);
	private Vector2 _previousPosition = new Vector2(200, 200);

	// Collision values
	private Rectangle _collisionBox;
	private float _collisionBoxSize;
	private const int _collisionBoxHeight = 10;
	private const float _playerScale = 0.15f;

	// Movement values
	private int _dashSpeed = 15;
	private int _speed = 5;

	// Ability bools
	private bool _canDash = true;
	private bool _dashing = false;

	// Durations
	private float _dashDuration = 0.1f;

	// Cooldowns
	private float _dashCooldown = 0.75f;

	public void Update(Vector2 inputAxis) {
		ProcessMovement(inputAxis);

		// Change player direction on direction input
		if (inputAxis.X != 0 || inputAxis.Y != 0) {
			_facingDirection = inputAxis;
		}
	}

	public void Draw(SpriteBatch spriteBatch) {
		spriteBatch.Draw(_texture, _position, null, Color.White, 0f,
					  	 new Vector2(0, 0), _playerScale, SpriteEffects.None, 0f);
	}

	public void Dash(Vector2 direction) {
		if (_canDash || _dashing) {
			// During first dash set _dashing to true
			if (_canDash) {
				_dashing = true;
			}
			_canDash = false;
			if (direction.X == 0  && direction.Y == 0) {
				direction = _facingDirection;
			}
			float dSpeed = (direction.X != 0 && direction.Y != 0) ?
						   (float) (_dashSpeed / 1.5) : _dashSpeed;
			_position.X += (direction.X * dSpeed);
			_position.Y += (direction.Y * dSpeed);
		}
	}

	// Collision Functions
	public void Collide(Rectangle collider) {
		_position = _previousPosition;
		// If wall is to the left or right of player, allow vertical movement
		if (collider.Left >= _position.X + (_texture.Width * _playerScale) ||
			collider.Right <= _position.X) {
			_position.Y = _collisionBox.Y - (float) (_texture.Height * _playerScale) + _collisionBoxHeight;
		}
		// If wall is above or below player, allow horizontal movement
		else if (collider.Top >= (int) (_position.Y + (_texture.Height * _playerScale)) ||
				 collider.Bottom <= _position.Y + (float) (_texture.Height * _playerScale) - _collisionBoxHeight) {
			_position.X = _collisionBox.X;
		}
		else  {
			Console.Write("Collider.Top: ");
			Console.Write(collider.Top);
			Console.WriteLine();
			Console.Write("position.Y + height: ");
			Console.Write((int) (_position.Y + (_texture.Height * _playerScale)));
			Console.WriteLine();
		}
	}

	// Timer Functions
	public Timer DashCooldownTimer() {
		Action cb = DashReady;
		Timer timer = new Timer(_dashCooldown, cb);
		return timer;
	}

	public Timer DashDurationTimer() {
		Action cb = DashComplete;
		Timer timer = new Timer(_dashDuration, cb);
		return timer;
	}

	// Getters
	public bool GetDash() {
		return (_canDash || _dashing);
	}

	public Vector2 GetPosition() {
		return _position;
	}

	public Rectangle GetCollider() {
		_collisionBox.X = (int) _position.X;
		_collisionBox.Y = (int) (_position.Y + _collisionBoxSize);
		return _collisionBox;
	}

	// Setters
	public void SetTexture(Texture2D texture) {
		_texture = texture;
		SetCollisionBox();
	}

	private void SetCollisionBox() {
		_collisionBoxSize = ((_texture.Height * _playerScale) - _collisionBoxHeight);
		_collisionBox = new Rectangle((int) _position.X,
									  (int) (_position.Y + _collisionBoxSize),
									  (int) (_texture.Width * _playerScale),
									  _collisionBoxHeight);
  	}

	private void ProcessMovement(Vector2 inputAxis) {
		_previousPosition = _position;
		if (_dashing) {
			Dash(_facingDirection);
		}
		else {
			_position.X += (inputAxis.X * _speed);
			_position.Y += (inputAxis.Y * _speed);
		}
	}

	// Set ability bool functions
	private void DashReady() {
		_canDash = true;
	}
	
	private void DashComplete() {
		_dashing = false;
	}
}
