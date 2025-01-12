using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core.Entities;
using Core.Systems;

namespace Core.Entities;
public class Enemy : Actor {
	protected Player _player;

	public Enemy() {
		_collisionBoxHeight = 8;
		_height = 40;
		_width = 30;
		Body = new Rectangle(200, 1000, _width, _height);
		CollisionBox = new Rectangle(_body.X,
									 _body.Y + _height - _collisionBoxHeight,
									 _body.Width, _body.Height);
		Speed = 2;
	}

	public void Update(Player player) {
		_player = player;
		_collisionBox.X = Body.X;
		_collisionBox.Y = Body.Y + _height - _collisionBoxHeight;
		ProcessMovement();
	}

	public override void Draw(SpriteBatch spriteBatch) {
		spriteBatch.Draw(Texture, Body, Color.Red);
	}

	protected void ProcessMovement(Vector2 direction) {
	}

	protected void ProcessMovement() {
		Vector2 direction;
		direction = new Vector2(0, 0);
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
