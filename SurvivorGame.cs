using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Character;
using Obstacle;
using Time;

namespace survivor_game;

public class SurvivorGame : Game {
    private GraphicsDeviceManager _graphics;

    private SpriteBatch _spriteBatch;

	// Input properties
    private Vector2 _inputAxis;

	// Player properties
    private Player _player;
    private Texture2D _playerTexture;

	// Obstacles
	private Wall _house;
	private Wall _wall;
	private Texture2D _wallTexture;
	private Texture2D _houseTexture;
	private Wall[] _obstacles = new Wall[2];

	// Timers
	private TimerManager _timerManager;
	private Timer _dashCooldownTimer;
	private Timer _dashDurationTimer;

    public SurvivorGame() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() {
        _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width / 2;
		_graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height / 2;
        _graphics.IsFullScreen = false;
        _graphics.ApplyChanges();

        _player = new Player();

		// Create timers and store in timerManager
		_dashCooldownTimer = _player.DashCooldownTimer();
		_dashDurationTimer = _player.DashDurationTimer();
		Timer[] timers = {_dashCooldownTimer, _dashDurationTimer};
		_timerManager = new TimerManager(timers);

        _inputAxis = new Vector2(0, 0);

        base.Initialize();
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _playerTexture = Content.Load<Texture2D>("player");
		_player.SetTexture(_playerTexture);

		_houseTexture = Content.Load<Texture2D>("house");
		_wallTexture = Content.Load<Texture2D>("rectangle");

		_house = new Wall(new Rectangle(400, 200, _houseTexture.Width, _houseTexture.Height));
		_wall = new Wall(new Rectangle(100, 400, _wallTexture.Width, _wallTexture.Height));

		_wall.SetTexture(_wallTexture);
		_house.SetTexture(_houseTexture);

		_obstacles[0] = _house;
		_obstacles[1] = _wall;
    }

    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			Keyboard.GetState().IsKeyDown(Keys.Escape)) {
            Exit();
		}

		// Update timerManager timers
		_timerManager.Update(gameTime);

		MoveInput();
		DashInput();

        _player.Update(_inputAxis);
		UpdateObstacles(_player);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        //displayFrames(gameTime);
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
		_house.Draw(_spriteBatch);
		_wall.Draw(_spriteBatch);
        _player.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void displayFrames(GameTime gameTime) {
        float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
        Console.Write("Frames: ");
        Console.WriteLine(frameRate);
    }

	private void MoveInput() {
		bool up = (Keyboard.GetState().IsKeyDown(Keys.W)) ? true : false;
        bool down = (Keyboard.GetState().IsKeyDown(Keys.S)) ? true : false;
        bool left = (Keyboard.GetState().IsKeyDown(Keys.A)) ? true : false;
        bool right = (Keyboard.GetState().IsKeyDown(Keys.D)) ? true : false;

        if ((up || down) && !(up && down)) {
            _inputAxis.Y = Keyboard.GetState().IsKeyDown(Keys.S) ? 1 : -1;
        }
        else {
            _inputAxis.Y = 0;
        }

        if ((left || right) && !(left && right)) {
            _inputAxis.X = Keyboard.GetState().IsKeyDown(Keys.A) ? -1 : 1;
        }
        else {
            _inputAxis.X = 0;
        }
	}

	private void DashInput() {
        if (Keyboard.GetState().IsKeyDown(Keys.Space)) {
			if (_player.GetDash()) {
				_player.Dash(_inputAxis);
				UpdateObstacles(_player);
				_dashCooldownTimer.Start();
				_dashDurationTimer.Start();
			}
		}
	}

	private void UpdateObstacles(Player player) {
		foreach (Wall obstacle in _obstacles) {
			obstacle.Update(player);
		}
	}
}
