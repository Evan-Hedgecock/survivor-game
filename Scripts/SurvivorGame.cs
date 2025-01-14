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
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

	// Grid and path nodes
	private GameGrid _gameGrid;
	private Node[,] _nodeGrid;

	private Pathfinder _pathfinder;

	// Input properties
    private Vector2 _inputAxis;

	// Textures
	private Texture2D _wallTexture;
    private Texture2D _playerTexture;
	private Texture2D _enemyTexture;

	// Player properties
    private readonly Player _player = new(new Vector2(-495, 973));

	// Enemy properties
	private Enemy _enemy;

	// Obstacles
	private readonly Wall _wall = new(new Vector2(100, 400), 200, 10);
	private readonly Wall _wall2 = new(new Vector2(300, 400), 10, 200);
	private readonly Wall _wall3 = new(new Vector2(100, 400), 10, 200);
	private Wall[] _walls;

	// Timers
	private TimerManager _timerManager;
	private Timer _dashCooldownTimer;
	private Timer _dashDurationTimer;

	private readonly Camera _camera;

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

		_walls = [_wall, _wall2, _wall3];

		// Initialize game grid
		int worldHeight = 2000;
		int worldWidth = 1000;
		_gameGrid = new GameGrid(worldHeight, worldWidth);
		_nodeGrid = _gameGrid.NodeGrid;
		_gameGrid.WorldPosToNode(_player.Position);
		foreach (Wall wall in _walls) {
			foreach(Node node in _gameGrid.WorldRectToNodes(wall.CollisionShape)) {
				try {
					node.Blocked = true;
				} catch (Exception) {}

			}
		}

		_pathfinder = new Pathfinder(_gameGrid);
		_enemy = new Enemy(_pathfinder, _gameGrid);

		// Create timers and store in timerManager
		_dashCooldownTimer = _player.DashCooldownTimer();
		_dashDurationTimer = _player.DashDurationTimer();
		Timer[] timers = [_dashCooldownTimer, _dashDurationTimer];
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

		foreach (Node node in _nodeGrid) {
			node.Texture = _playerTexture;
		}

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

        _player.Update(_inputAxis, _walls);
		_enemy.Update(_player);
		_camera.Update(new Vector2(_player.Body.X, _player.Body.Y), GraphicsDevice);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        //DisplayFrames(gameTime);
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(transformMatrix: _camera.CreateMatrix(GraphicsDevice));
		foreach (Node node in _nodeGrid) {
			node.Draw(_spriteBatch);
		}
		DrawWalls(_spriteBatch);
        _player.Draw(_spriteBatch);
		_enemy.Draw(_spriteBatch);
        _spriteBatch.End();
        base.Draw(gameTime);
    }

    private static void DisplayFrames(GameTime gameTime) {
        float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
        Console.Write("Frames: ");
        Console.WriteLine(frameRate);
    }

	private void MoveInput() {
		bool up = Keyboard.GetState().IsKeyDown(Keys.W);
        bool down = Keyboard.GetState().IsKeyDown(Keys.S);
        bool left = Keyboard.GetState().IsKeyDown(Keys.A);
        bool right = Keyboard.GetState().IsKeyDown(Keys.D);

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
				_player.Dash(_walls);
				_dashCooldownTimer.Start();
				_dashDurationTimer.Start();
			}
		}
	}

	private void DrawWalls(SpriteBatch spriteBatch) {
		foreach (Wall wall in _walls) {
			wall.Draw(spriteBatch);
		}
	}
}
