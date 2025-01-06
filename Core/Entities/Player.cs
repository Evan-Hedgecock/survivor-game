using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core.Entities;
using Core.Systems;

namespace Core.Entities;
public class Player : Actor {
	// Movement values
	private int _dashSpeed = 15;

	// Ability bools
	private bool _canDash = true;
	private bool _dashing = false;

	// Durations
	private float _dashDuration = 0.1f;

	// Cooldowns
	private float _dashCooldown = 0.75f;

	public Player(Vector2 position) {
		// Positional values
		Position = position;
		_facingDirection = new Vector2(1, 0);
		_previousPosition = Position;

		// Movement values
		_speed = 5;

		// Collision values
		_collisionBoxHeight = 10;
		_scale = 0.15f;
	}

	public void Update(Vector2 inputAxis) {
		ProcessMovement(inputAxis);
		_collisionBox.X = (int) _position.X;
		_collisionBox.Y = (int) (_position.Y + _collisionBoxOffset);

		// Change player direction on direction input
		if (inputAxis.X != 0 || inputAxis.Y != 0) {
			_facingDirection = inputAxis;
		}
	}

	public override void Draw(SpriteBatch spriteBatch) {
		spriteBatch.Draw(Texture, _position, null, Color.White, 0f,
					  	 new Vector2(0, 0), _scale, SpriteEffects.None, 0f);
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

	protected override void ProcessMovement(Vector2 direction) {
		_previousPosition = _position;
		if (_dashing) {
			Dash(_facingDirection);
		}
		else {
			_position.X += (direction.X * _speed);
			_position.Y += (direction.Y * _speed);
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
