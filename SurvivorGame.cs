using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Character;

namespace survivor_game;

public class SurvivorGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Vector2 inputAxis;

    private Player player;
    private Texture2D playerTexture;

    public SurvivorGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        inputAxis = new Vector2(0, 0);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        playerTexture = Content.Load<Texture2D>("player");
        player = new Player(playerTexture, new Vector2(200, 200), 5);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        bool up = (Keyboard.GetState().IsKeyDown(Keys.W)) ? true : false;
        bool down = (Keyboard.GetState().IsKeyDown(Keys.S)) ? true : false;
        bool left = (Keyboard.GetState().IsKeyDown(Keys.A)) ? true : false;
        bool right = (Keyboard.GetState().IsKeyDown(Keys.D)) ? true : false;
        if (up || down)
        {
            inputAxis.Y = Keyboard.GetState().IsKeyDown(Keys.S) ? 1 : -1;
        }
        else
        {
            inputAxis.Y = 0;
        }

        if (left || right)
        {
            inputAxis.X = Keyboard.GetState().IsKeyDown(Keys.A) ? -1 : 1;
        }
        else
        {
            inputAxis.X = 0;
        }

        player.Update(inputAxis);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        player.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
