using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Core.Character;
using Core.Objects;
using Core.Systems;
using Core.Physics;
using Core;

namespace Scripts;
public class SurvivorGame : Game {
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
	private int _height;
	private int _width;

	// Grid and path nodes
	private Grid _gameGrid;
	private Node[,] _nodeGrid;

	private Pathfinder _pathfinder;

	// Input properties
    private Vector2 _inputAxis;

	// Textures
	private Texture2D _wallTexture;
    private Texture2D _playerTexture;
	private Texture2D _enemyTexture;

	// Player properties
    private Player _player;

	// Enemy properties

	// Obstacles
	private static StaticObject _wall = new(new Rectangle(-250, -90, 50, 60));
	private static StaticObject _wall2 = new(new Rectangle(-50, -10, 210, 50));
	private static StaticObject _wall4 = new(new Rectangle(-40, 0, 50, 200));
	private static StaticObject _wall3 = new(new Rectangle(-180, 100, 100, 100));
	private static StaticObject[] _walls = [_wall, _wall2, _wall3, _wall4];
		
	

	// Timers
	private TimerManager _timerManager;
	private Timer _dashCooldownTimer;
	private Timer _dashDurationTimer;

	private readonly Camera _camera;
	private CollisionManager _collisionManager;

    public SurvivorGame() {
        _graphics = new GraphicsDeviceManager(this);
		_camera = new Camera(new Vector2(0, 0));
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() {
        _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width / 2;
		_graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height / 2;
        _graphics.IsFullScreen = false;
        _graphics.ApplyChanges();

		Global.Services = Services;
		_collisionManager = new CollisionManager([.. _walls]);
		Services.AddService(typeof(CollisionManager), _collisionManager);

		_player = new(new Rectangle(100, -50, 20, 40));
		// Initialize game grid
		int worldHeight = 500;
		int worldWidth = 500;
		int cellSize = 20;
		_gameGrid = new Grid(worldHeight, worldWidth, cellSize);
		_nodeGrid = _gameGrid.NodeGrid;
		// Set nodes Blocked to true where a wall or other collider is

		_collisionManager.Initialize();
		_player.Initialize();

		_pathfinder = new Pathfinder(_gameGrid);
		//_enemy = new Enemy(_pathfinder, _gameGrid);

		// Create timers and store in timerManager
		//_dashCooldownTimer = _player.DashCooldownTimer();
		//_dashDurationTimer = _player.DashDurationTimer();
		//Timer[] timers = [_dashCooldownTimer, _dashDurationTimer];
		//_timerManager = new TimerManager(timers);


        _inputAxis = new Vector2(0, 0);

        base.Initialize();
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

		// Load Textures
        _playerTexture = Content.Load<Texture2D>("player");
		_wallTexture = Content.Load<Texture2D>("rectangle");
		_enemyTexture = Content.Load<Texture2D>("player");

		foreach (Node node in _nodeGrid) {
			node.Texture = _playerTexture;
		}

		// Create Textures
		_player.Texture = _playerTexture;
		//_enemy.Texture = _enemyTexture;
		_wall.Texture = _wallTexture;
		_wall2.Texture = _wallTexture;
		_wall3.Texture = _wallTexture;
		_wall4.Texture = _wallTexture;
    }

    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			Keyboard.GetState().IsKeyDown(Keys.Escape)) {
            Exit();
		}
		//_collisionManager.Update();
		// Update timerManager timers
		//_timerManager.Update(gameTime);

		MoveInput();
		Console.WriteLine(_inputAxis);
		//DashInput();

        _player.Update(_inputAxis, gameTime);
		//_enemy.Update(_player);
		_camera.Update(new Vector2(_player.PositionX, _player.PositionY), GraphicsDevice);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        //DisplayFrames(gameTime);
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(transformMatrix: _camera.CreateMatrix(GraphicsDevice));
		//_enemy.Draw(_spriteBatch);
		foreach (Node node in _nodeGrid) {
			node.Draw(_spriteBatch);
		}
		DrawWalls(_spriteBatch);
        _player.Draw(_spriteBatch);
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

//	private void DashInput() {
//        if (Keyboard.GetState().IsKeyDown(Keys.Space)) {
//			if (_player.GetDash()) {
//				_player.Dash(_walls);
//				_dashCooldownTimer.Start();
//				_dashDurationTimer.Start();
//			}
//		}
//	}

	private void DrawWalls(SpriteBatch spriteBatch) {
		foreach (StaticObject wall in _walls) {
			wall.Draw(spriteBatch);
		}
	}
}
