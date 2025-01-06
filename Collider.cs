using Microsoft.Xna.Framework;
using Character;

namespace Collider;
public abstract class StaticCollider {
	protected Rectangle _collisionShape;

	public void Update(Player player) {
		while (CheckCollision(player.GetCollider()))
			player.Collide(_collisionShape);
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
