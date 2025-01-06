using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core.Entities;

namespace Enemy;
public class Monster {
	private Texture2D _texture { get; set; }

	// Positional values
	private Vector2 _position = new Vector2(300, 100);
	private Vector2 _previousPosition = new Vector2(300, 100);

	// Collision values
	private Rectangle _collisionBox;
	private float _collisionBoxSize;
	private const int _collisionBoxHeight = 10;
	private float _scale = 0.10f;

	// Movement values
	private int _speed = 2;
	private Vector2 _moveToward;

	public void Update(Player player) {
		_collisionBox.X = (int) _position.X;
		_collisionBox.Y = (int) (_position.Y + _collisionBoxSize);
		Vector2 playerCenter = player.CollisionBox.Center.ToVector2();
		Vector2 centerPosition = _collisionBox.Center.ToVector2();
		_moveToward = Vector2.Subtract(playerCenter, centerPosition);
		ProcessMovement();
	}

	public void Draw(SpriteBatch spriteBatch) {
		spriteBatch.Draw(_texture, _position, null, Color.Red, 0f,
						 new Vector2(0, 0), _scale, SpriteEffects.None, 0f);
	}

	public void SetTexture(Texture2D texture) {
		_texture = texture;
		SetCollisionBox();
	}

	public void SetCollisionBox() {
		_collisionBoxSize = ((_texture.Height * _scale) - _collisionBoxHeight);
		_collisionBox = new Rectangle((int) _position.X,
									  (int) (_position.Y + _collisionBoxSize),
									  (int) (_texture.Width * _scale),
									  _collisionBoxHeight);
	}

	private void ProcessMovement() {
		if (_moveToward.X < 0) {
			_position.X -= _speed;
		}
		else if (_moveToward.X > 0) {
			_position.X += _speed;
		}

		if (_moveToward.Y < 0) {
			_position.Y -= _speed;
		}
		else if (_moveToward.Y > 0) {
			_position.Y += _speed;
		}
	}
}
