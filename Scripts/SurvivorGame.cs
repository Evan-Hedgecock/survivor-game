using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Core.Entities;
using Core.Objects;
using Core.Systems;

namespace Scripts;
public class SurvivorGame : Game {
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

	// Input properties
    private Vector2 _inputAxis;

	// Textures
	private Texture2D _wallTexture;
	private Texture2D _houseTexture;
    private Texture2D _playerTexture;
	private Texture2D _enemyTexture;

	// Player properties
    private Player _player = new Player(new Vector2(200, 200));

	// Enemy properties
	private Enemy _enemy;

	// Obstacles
	private Wall _wall = new Wall(new Vector2(100, 400), 200, 10);
	private Wall _wall2 = new Wall(new Vector2(300, 400), 10, 200);
	private Wall _wall3 = new Wall(new Vector2(100, 400), 10, 200);
	private Wall[] _obstacles;

	// Timers
	private TimerManager _timerManager;
	private Timer _dashCooldownTimer;
	private Timer _dashDurationTimer;

	private Camera _camera;

    public SurvivorGame() {
        _graphics = new GraphicsDeviceManager(this);
		_camera = new Camera(new Vector2 (_player.Body.X, _player.Body.Y));
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() {
        _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width / 2;
		_graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height / 2;
        _graphics.IsFullScreen = false;
        _graphics.ApplyChanges();

		_obstacles = new Wall[] {_wall, _wall2, _wall3};

		_enemy = new Enemy();

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

		// Load Textures
        _playerTexture = Content.Load<Texture2D>("player");
		_houseTexture = Content.Load<Texture2D>("house");
		_wallTexture = Content.Load<Texture2D>("rectangle");
		_enemyTexture = Content.Load<Texture2D>("player");

		// Create Textures
		_player.Texture = _playerTexture;
		_enemy.Texture = _enemyTexture;
		_wall.CreateTexture(_wallTexture);
		_wall2.CreateTexture(_wallTexture);
		_wall3.CreateTexture(_wallTexture);
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

        _player.Update(_inputAxis, _obstacles);
		_enemy.Update(_player);

		_camera.Update(new Vector2(_player.Body.X, _player.Body.Y), GraphicsDevice);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        //DisplayFrames(gameTime);
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(transformMatrix: _camera.CreateMatrix(GraphicsDevice));
		DrawObstacles(_spriteBatch);
        _player.Draw(_spriteBatch);
		_enemy.Draw(_spriteBatch);
        _spriteBatch.End();
        base.Draw(gameTime);
    }

    private void DisplayFrames(GameTime gameTime) {
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
				_player.Dash(_inputAxis, _obstacles);
				_dashCooldownTimer.Start();
				_dashDurationTimer.Start();
			}
		}
	}

	private void DrawObstacles(SpriteBatch spriteBatch) {
		foreach (Wall obstacle in _obstacles) {
			obstacle.Draw(spriteBatch);
		}
	}
}
