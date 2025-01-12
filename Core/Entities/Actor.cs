using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core.Objects;

namespace Core.Entities;
public abstract class Actor {
	public virtual Texture2D Texture { 
		get { return _texture; }
		set { _texture = value; }
	}
	protected Texture2D _texture;
	protected float _scale;

	public virtual Rectangle Body {
		get { return _body; }
		protected set { _body = value; }
	}
	protected Rectangle _body;

	protected int _height;
	protected int _width;

	public virtual Rectangle CollisionBox {
		get { return _collisionBox; }
		protected set { _collisionBox = value; }
	}
	protected Rectangle _collisionBox;
	protected int _collisionBoxHeight;

	protected Vector2 _facingDirection;

	// Movement Values
	public virtual int Speed { 
		get { return _speed; }
	   	set { _speed = value; }
	}
	protected int _speed;

	protected Vector2 CheckCollisions(Vector2 direction, Wall[] walls) {
		Rectangle[] collisionBoxes = EstimateCollisionBox(direction, _speed);
		return CalculateMoveDirection(walls, collisionBoxes);
	}

	protected Vector2 CheckCollisions(Vector2 direction, Wall[] walls, float speed) {
		Rectangle[] collisionBoxes = EstimateCollisionBox(direction, speed);
		return CalculateMoveDirection(walls, collisionBoxes);
	}

	protected Rectangle[] EstimateCollisionBox(Vector2 direction, float speed) {
		Rectangle xCollision = new Rectangle(
			_collisionBox.X + (int) (direction.X * speed),
			_collisionBox.Y,
			_collisionBox.Width,
			_collisionBox.Height);
		Rectangle yCollision = new Rectangle(
			_collisionBox.X,
			_collisionBox.Y + (int) (direction.Y * speed),
			_collisionBox.Width,
			_collisionBox.Height);
		return new Rectangle[] {xCollision, yCollision};
	}

	protected Vector2 CalculateMoveDirection(Wall[] walls, Rectangle[] collisionBoxes) {
		Vector2 moveDirection = new Vector2(1, 1);
		foreach (Wall wall in walls) {
			if (wall.CollisionShape.Intersects(collisionBoxes[0])) {
				moveDirection.X = 0;
			} 
			if (wall.CollisionShape.Intersects(collisionBoxes[1])) {
				moveDirection.Y = 0;
			}
			if (moveDirection.X == 0 && moveDirection.Y == 0) {
				return moveDirection;
			}
		}
		return moveDirection;
	}

	public abstract void Draw(SpriteBatch spriteBatch);
}



