using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Grid;

namespace algo;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

	private Texture2D _nodeTexture;

	private Node[,] Grid;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {

		_graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width / 2;
		_graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height / 2;
		_graphics.ApplyChanges();
		var size = 100;

		Grid = new Node[size, size];


		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				var node = new Node(new Vector2(50 * i, 50 * j));
				Grid[i, j] = node;
			}
		}

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

		_nodeTexture = Content.Load<Texture2D>("square");
		foreach (Node node in Grid) {
			node.Texture = _nodeTexture;
		}


        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

		_spriteBatch.Begin();
		int count = 0;
		for (int i = 0; i < Grid.GetLength(0); i++) {
			count++;
			for (int j = 0; j < Grid.GetLength(1); j++) {
				Grid[i, j].Draw(_spriteBatch, count);
				count++;
			}
		}
		_spriteBatch.End();


        base.Draw(gameTime);
    }
}
