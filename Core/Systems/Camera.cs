using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Systems;
public class Camera {

	public Vector2 Position {
		get { return _position; }
		set { _position = value; }
	}
	private Vector2 _position;

	public Camera(Vector2 position) {
		Position = position;
	}

	public void Update(Vector2 position, GraphicsDevice graphicsDevice) {
		Position = Vector2.Lerp(Position, position , 0.3f);
	}

	public Matrix CreateMatrix(GraphicsDevice graphicsDevice) {
		return Matrix.CreateTranslation(new Vector3(-Position, 0)) *
			   Matrix.CreateTranslation(new Vector3(
					graphicsDevice.Viewport.Width / 2f,
					graphicsDevice.Viewport.Height / 2f,
					0));
	}
}	
