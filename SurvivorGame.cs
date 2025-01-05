using Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Time;

namespace survivor_game;

public class SurvivorGame : Game
{
    private GraphicsDeviceManager _graphics;

    private SpriteBatch _spriteBatch;


    private Vector2 inputAxis;

    private Player player;
    private Texture2D playerTexture;

	// Timers
	private TimerManager _timerManager;
	private Timer dashCooldownTimer;
	private Timer dashDurationTimer;

    public SurvivorGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width / 2;
		_graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height / 2;
        _graphics.IsFullScreen = false;
        _graphics.ApplyChanges();

        player = new Player();

		// Create timers and store in timerManager
		dashCooldownTimer = player.dashCooldownTimer();
		dashDurationTimer = player.dashDurationTimer();
		Timer[] timers = {dashCooldownTimer, dashDurationTimer};
		_timerManager = new TimerManager(timers);

        inputAxis = new Vector2(0, 0);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        playerTexture = Content.Load<Texture2D>("rectangle");
		player.setTexture(playerTexture);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

		// Update timerManager timers
		_timerManager.Update(gameTime);

        bool up = (Keyboard.GetState().IsKeyDown(Keys.W)) ? true : false;
        bool down = (Keyboard.GetState().IsKeyDown(Keys.S)) ? true : false;
        bool left = (Keyboard.GetState().IsKeyDown(Keys.A)) ? true : false;
        bool right = (Keyboard.GetState().IsKeyDown(Keys.D)) ? true : false;

        if ((up || down) && !(up && down))
        {
            inputAxis.Y = Keyboard.GetState().IsKeyDown(Keys.S) ? 1 : -1;
        }
        else
        {
            inputAxis.Y = 0;
        }

        if ((left || right) && !(left && right))
        {
            inputAxis.X = Keyboard.GetState().IsKeyDown(Keys.A) ? -1 : 1;
        }
        else
        {
            inputAxis.X = 0;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
			// On dash, call the player method that instantiates timer and assign to variable
			// Pass that variable to the timerManager object

			if (player.getDash())
			{
				player.dash(inputAxis);
				dashCooldownTimer.start();
				dashDurationTimer.start();
			}
		}

        player.Update(inputAxis);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        //displayFrames(gameTime);
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        player.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void displayFrames(GameTime gameTime)
    {
        float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
        Console.Write("Frames: ");
        Console.WriteLine(frameRate);
    }
}
