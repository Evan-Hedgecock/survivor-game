using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core.Entities;
using Core.Utils;

namespace Core.Entities;
public class Enemy : Actor {

	public Enemy(Pathfinder pathfinder) {
		_collisionBoxHeight = 8;
		_height = 40;
		_width = 30;
		Body = new Rectangle(400, 500, _width, _height);
		CollisionBox = new Rectangle(_body.X,
									 _body.Y + _height - _collisionBoxHeight,
									 _body.Width, _body.Height);
		_scale = 0.12f;
		Speed = 2;
		Pfinder = pathfinder;
	}

	public void Update(Player player) {
		Console.WriteLine("Finding target");
		Pfinder.FindTargetCell(player.CollisionBox);
		Console.WriteLine("Finding start");
		Console.WriteLine(CollisionBox);
		Pfinder.FindStartCell(_collisionBox);
		Console.WriteLine("Finding path");
		ProcessMovement(Pfinder.FindPath());
		_collisionBox.X = Body.X;
		_collisionBox.Y = Body.Y + _height - _collisionBoxHeight;

	}

	public override void Draw(SpriteBatch spriteBatch) {
		spriteBatch.Draw(Texture, Body, Color.Red);
	}

	protected void ProcessMovement(Vector2 direction) {
	}

	protected void ProcessMovement(List<Vector2> path) {
		Vector2 direction;
		try {
			direction = new Vector2(path[1].X - CollisionBox.X,
									path[1].Y - CollisionBox.Y);
		}
		catch (Exception) {
			direction = new Vector2(0, 0);
		}
 		if (direction.X > 0) {
 			_body.X += Speed;
 		}
 		else if (direction.X < 0) {
 			_body.X -= Speed;
 		}
 
 		if (direction.Y > 0) {
 			_body.Y += Speed;
 		}
 		else if (direction.Y < 0) {
 			_body.Y -= Speed;
		}
	}
}
