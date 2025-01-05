using Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Obstacle;
using System;
using Time;
using Weapon;

namespace survivor_game;

public class SurvivorGame : Game
{
    private GraphicsDeviceManager _graphics;

    private SpriteBatch _spriteBatch;

	// Input properties
    private Vector2 inputAxis;

	// Player properties
    private Player player;
    private Texture2D playerTexture;

	// Weapon properties
	private Gun gun;
	private Texture2D gunTexture;

	// Obstacles
	private Wall house;
	private Wall wall;
	private Texture2D wallTexture;
	private Texture2D houseTexture;

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

		gun = new Gun();


		// Create timers and store in timerManager
		dashCooldownTimer = player.DashCooldownTimer();
		dashDurationTimer = player.DashDurationTimer();
		Timer[] timers = {dashCooldownTimer, dashDurationTimer};
		_timerManager = new TimerManager(timers);

        inputAxis = new Vector2(0, 0);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        playerTexture = Content.Load<Texture2D>("player");
		player.SetTexture(playerTexture);

		gunTexture = Content.Load<Texture2D>("gun");
		gun.setTexture(gunTexture);

		houseTexture = Content.Load<Texture2D>("house");
		wallTexture = Content.Load<Texture2D>("rectangle");

		house = new Wall(new Rectangle(400, 200, houseTexture.Width, houseTexture.Height));
		wall = new Wall(new Rectangle(100, 400, wallTexture.Width, wallTexture.Height));

		wall.SetTexture(wallTexture);
		house.SetTexture(houseTexture);

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

			if (player.GetDash())
			{
				player.Dash(inputAxis);
				house.Update(player);
				wall.Update(player);
				dashCooldownTimer.Start();
				dashDurationTimer.Start();
			}
		}

        player.Update(inputAxis);
		gun.Update(player.GetPosition());
		house.Update(player);
		wall.Update(player);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        //displayFrames(gameTime);
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
		gun.Draw(_spriteBatch);
		house.Draw(_spriteBatch);
		wall.Draw(_spriteBatch);
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
