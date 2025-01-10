using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core.Objects;
using Core.Utils;

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

	protected List<Vector2> _path;

	public Pathfinder Pfinder {
		protected get { return _pathfinder; }
		set { _pathfinder = value; }
	}
	protected Pathfinder _pathfinder;

	// Movement Values
	public virtual int Speed { 
		get { return _speed; }
	   	set { _speed = value; }
	}
	protected int _speed;

	protected Vector2 CheckCollisions(Vector2 direction, Wall[] walls) {
		Vector2 moveDirection = new Vector2(1, 1);
		Rectangle xCollision = new Rectangle(
			_collisionBox.X + (int) (direction.X * _speed),
			_collisionBox.Y,
			_collisionBox.Width,
			_collisionBox.Height);
		Rectangle yCollision = new Rectangle(
			_collisionBox.X,
			_collisionBox.Y + (int) (direction.Y * _speed),
			_collisionBox.Width,
			_collisionBox.Height);

		foreach (Wall wall in walls) {
			if (wall.CollisionShape.Intersects(xCollision)) {
				moveDirection.X = 0;
			}
			if (wall.CollisionShape.Intersects(yCollision)) {
				moveDirection.Y = 0;
			}
			if (moveDirection.X == 0 && moveDirection.Y == 0) {
				break;
			}
		}
		return moveDirection;
	}

	protected Vector2 CheckCollisions(Vector2 direction, Wall[] walls, float speed) {
		Vector2 moveDirection = new Vector2(1, 1);
		Rectangle xCollision = new Rectangle(
			_collisionBox.X + (int) (direction.X * speed) + (int) (direction.X * 1),
			_collisionBox.Y + (int) (direction.Y * 1),
			_collisionBox.Width,
			_collisionBox.Height);
		Rectangle yCollision = new Rectangle(
			_collisionBox.X + (int) (direction.X * 1),
			_collisionBox.Y + (int) (direction.Y * speed) + (int) (direction.Y * 1),
			_collisionBox.Width,
			_collisionBox.Height);

		foreach (Wall wall in walls) {
			if (wall.CollisionShape.Intersects(xCollision)) {
				moveDirection.X = 0;
			} 
			if (wall.CollisionShape.Intersects(yCollision)) {
				moveDirection.Y = 0;
			}
			if (moveDirection.X == 0 && moveDirection.Y == 0) {
				break;
			}
		}
		return moveDirection;
	}

	public abstract void Draw(SpriteBatch spriteBatch);
}



