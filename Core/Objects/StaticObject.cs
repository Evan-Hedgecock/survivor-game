using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core.Entities;

namespace Core.Objects;
public abstract class StaticObject {

	public virtual Texture2D Texture {
		get { return _texture; }
		set { _texture = value; }
	}
	private Texture2D _texture;

	public virtual Vector2 Position {
		get { return _position; }
		set { _position = value; }
	}
	private Vector2 _position;

	public virtual Rectangle CollisionShape {
		get { return _collisionShape; }
		set { _collisionShape = value; }
	}
	private Rectangle _collisionShape;

	public void Update(Actor actor) {
		while (CheckCollision(actor.CollisionBox)) {
			actor.Collide(_collisionShape);
		}
	}

	public bool CheckCollision(Rectangle collider) {
		return _collisionShape.Intersects(collider);
	}

	public bool CheckCollision(Point point) {
		return _collisionShape.Contains(point);
	}

	private bool CheckContaining(Rectangle collider) {
		return _collisionShape.Contains(collider);
	}
}
