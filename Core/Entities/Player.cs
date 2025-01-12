using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core.Entities;
using Core.Objects;
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
		_height = 40;
		_width = 20;
		Body = new Rectangle((int) position.X, (int) position.Y, _width, _height);
		_facingDirection = new Vector2(1, 0);

		// Movement values
		_speed = 5;

		// Collision values
		_collisionBoxHeight = 10;
		CollisionBox = new Rectangle(_body.X, _body.Y + _height - _collisionBoxHeight,
									 _body.Width, _collisionBoxHeight);
	}

	public void Update(Vector2 inputAxis, Wall[] walls) {
		_collisionBox.X = _body.X;
		_collisionBox.Y = _body.Y + _height - _collisionBoxHeight;
		ProcessMovement(inputAxis, walls);

		// Change player direction on direction input
		if (inputAxis.X != 0 || inputAxis.Y != 0) {
			_facingDirection = inputAxis;
		}
	}

	public override void Draw(SpriteBatch spriteBatch) {
		spriteBatch.Draw(Texture, Body, Color.White);
	}

	public void Dash(Wall[] walls) {
		if (_canDash || _dashing) {
			// During first dash set _dashing to true
			if (_canDash) {
				_dashing = true;
			}
			_canDash = false;
			float dSpeed = (_facingDirection.X != 0 && _facingDirection.Y != 0) ?
						   (float) (_dashSpeed / 1.5) : _dashSpeed;
			Vector2 moveDirection = CheckCollisions(_facingDirection, walls, dSpeed);
			Console.WriteLine(moveDirection);
			_body.X += (int) (moveDirection.X * (_facingDirection.X * dSpeed));
			_body.Y += (int) (moveDirection.Y * (_facingDirection.Y * dSpeed));
			_collisionBox.X = _body.X;
			_collisionBox.Y = _body.Y + _height - _collisionBoxHeight;
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

	protected void ProcessMovement(Vector2 direction, Wall[] walls) {
		if (_dashing) {
			Dash(walls);
		}
		else {
			Vector2 moveDirection = CheckCollisions(direction, walls);
			_body.X += (int) (moveDirection.X * (direction.X * _speed));
			_body.Y += (int) (moveDirection.Y * (direction.Y * _speed));
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
