using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core.Entities;
using Core.Utils;

namespace Core.Entities;
public class Enemy : Actor {

	public Enemy(Pathfinder pathfinder) {
		Position = new Vector2(300, 100);
		_previousPosition = Position;
		_collisionBoxOffset = 10;
		_collisionBoxHeight = 8;
		_scale = 0.12f;
		Speed = 2;
		Pfinder = pathfinder;
	}

	public void Update(Player player) {
		_collisionBox.X = (int) _position.X;
		_collisionBox.Y = (int) (_position.Y + _collisionBoxOffset);

		Pfinder.FindTargetCell(player.Position);
		Pfinder.FindStartCell(Position);
		ProcessMovement(Pfinder.FindPath());
		UpdateCenter();
	}

	public override void Draw(SpriteBatch spriteBatch) {
		spriteBatch.Draw(Texture, Position, null, Color.Red, 0f,
						 new Vector2(0, 0), _scale, SpriteEffects.None, 0f);
	}

	protected override void ProcessMovement(Vector2 direction) {
	}

	protected void ProcessMovement(List<Vector2> path) {
		_previousPosition = Position;
		Vector2 direction;
		try {
			direction = new Vector2(path[1].X - Position.X,
									path[1].Y - Position.Y);
		}
		catch (Exception) {
			direction = new Vector2(0, 0);
		}
 		if (direction.X > 0) {
 			_position.X += Speed;
 		}
 		else if (direction.X < 0) {
 			_position.X -= Speed;
 		}
 
 		if (direction.Y > 0) {
 			_position.Y += Speed;
 		}
 		else if (direction.Y < 0) {
 			_position.Y -= Speed;
		}
	}
}
