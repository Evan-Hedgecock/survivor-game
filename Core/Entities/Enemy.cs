using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core.Entities;

namespace Core.Entities;
public class Enemy : Actor {

	// Movement values
	private Vector2 _moveToward;

	public Enemy() {
		Position = new Vector2(300, 100);
		_previousPosition = Position;
		_collisionBoxOffset = 10;
		_scale = 0.12f;
		Speed = 2;
	}

	public void Update(Player player) {
		_collisionBox.X = (int) _position.X;
		_collisionBox.Y = (int) (_position.Y + _collisionBoxOffset);
		Vector2 playerCenter = player.CollisionBox.Center.ToVector2();
		Vector2 centerPosition = _collisionBox.Center.ToVector2();
		_moveToward = Vector2.Subtract(playerCenter, centerPosition);
		ProcessMovement(_moveToward);
	}

	public override void Draw(SpriteBatch spriteBatch) {
		spriteBatch.Draw(Texture, _position, null, Color.Red, 0f,
						 new Vector2(0, 0), _scale, SpriteEffects.None, 0f);
	}

	protected override void ProcessMovement(Vector2 direction) {
		if (direction.X < 0) {
			_position.X -= Speed;
		}
		else if (direction.X > 0) {
			_position.X += Speed;
		}

		if (direction.Y < 0) {
			_position.Y -= Speed;
		}
		else if (direction.Y > 0) {
			_position.Y += Speed;
		}
	}
}
