using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Core.Entity;
using Core.Objects;
using Core.Systems;
using Core.Physics;
using Core;
using System.Collections.Generic;

namespace Scripts;
public class SurvivorGame : Game
{
	private readonly GraphicsDeviceManager _graphics;
	private SpriteBatch _spriteBatch;
	private int _height;
	private int _width;

	// Grid and path nodes
	private Grid _gameGrid;
	private Node[,] _nodeGrid;

	private List<Node> pathNodes;

	private Pathfinder _pathfinder;

	// Input properties
	private Vector2 _inputAxis = new(0, 0);

	// Textures
	private Texture2D _wallTexture;
	private Texture2D _playerTexture;
	private Texture2D _enemyTexture;

	// Player properties
	private Player _player;

	// Enemy properties
	private Enemy _enemy;

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
	private DamageManager _damageManager;

	public SurvivorGame()
	{
		_graphics = new GraphicsDeviceManager(this);
		_camera = new Camera(new Vector2(0, 0));
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
	}

	protected override void Initialize()
	{
		_graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width / 2;
		_graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height / 2;
		_graphics.IsFullScreen = false;
		_graphics.ApplyChanges();


		// Initialize game grid
		int worldHeight = 500;
		int worldWidth = 500;
		int cellSize = 20;
		_gameGrid = new Grid(worldHeight, worldWidth, cellSize);
		_nodeGrid = _gameGrid.NodeGrid;
		// Set nodes Blocked to true where a wall or other collider is

		// Create characters
		_player = new(new Rectangle(100, -50, 20, 40));
		_enemy = new(new Rectangle(0, -100, 15, 30), _gameGrid);

		List<GameObject> dynamicObjects = [_enemy, _player];
		List<GameObject> staticObjects = [.. _walls];
		List<Character> characterList = [_enemy, _player];

		foreach (GameObject obj in staticObjects)
		{
			Node[] nodes = _gameGrid.WorldRectToNodes(obj.CollisionBox);
			foreach (Node node in nodes)
			{
				node.Blocked = true;
			}
		}

		// Create and initialize services
		_collisionManager = new CollisionManager(staticObjects, dynamicObjects);
		_collisionManager.Initialize();
		_damageManager = new DamageManager(characterList);
		_damageManager.Initialize();
		_pathfinder = new Pathfinder(_gameGrid);
		_timerManager = new TimerManager();

		// Add services
		Global.Services = Services;
		Services.AddService(typeof(CollisionManager), _collisionManager);
		Services.AddService(typeof(DamageManager), _damageManager);
		Services.AddService(typeof(Pathfinder), _pathfinder);
		Services.AddService(typeof(TimerManager), _timerManager);

		// Initialize characters
		_player.Initialize();
		_enemy.Initialize();

		base.Initialize();
	}

	protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);

		// Load Textures
		_playerTexture = Content.Load<Texture2D>("player");
		_wallTexture = Content.Load<Texture2D>("rectangle");
		_enemyTexture = Content.Load<Texture2D>("player");

		foreach (Node node in _nodeGrid)
		{
			node.Texture = _playerTexture;
		}

		// Assign textures
		_player.Texture = _playerTexture;
		_player.HealthBar.LoadTextures(_playerTexture, _playerTexture);
		_enemy.Texture = _enemyTexture;
		_enemy.HealthBar.LoadTextures(_playerTexture, _playerTexture);
		_wall.Texture = _wallTexture;
		_wall2.Texture = _wallTexture;
		_wall3.Texture = _wallTexture;
		_wall4.Texture = _wallTexture;
	}

	protected override void Update(GameTime gameTime)
	{
		if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			Keyboard.GetState().IsKeyDown(Keys.Escape))
		{
			Exit();
		}
		// Update timerManager timers
		//_timerManager.Update(gameTime);

		// Update input
		MoveInput();
		AttackInput();
		//DashInput();

		// Update characters
		_player.Update(_inputAxis, gameTime);
		_enemy.Update(_player, gameTime);
		_collisionManager.Update();
		_damageManager.Update();

		Node start = _gameGrid.WorldPosToNode(new Vector2(_enemy.CollisionBoxX,
														  _enemy.CollisionBoxY));
		Node target = _gameGrid.WorldPosToNode(new Vector2(_player.CollisionBoxX,
														  _player.CollisionBoxY));
		List<Vector2> path = _pathfinder.FindPath(start, target);
		pathNodes = [];
		foreach (Vector2 waypoint in path)
		{
			pathNodes.Add(_gameGrid.WorldPosToNode(waypoint));
		}

		// Update Systems
		_camera.Update(new Vector2(_player.PositionX, _player.PositionY), GraphicsDevice);

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		//DisplayFrames(gameTime);
		GraphicsDevice.Clear(Color.CornflowerBlue);
		_spriteBatch.Begin(transformMatrix: _camera.CreateMatrix(GraphicsDevice));
		foreach (Node node in _nodeGrid)
		{
			if (pathNodes.Contains(node))
			{
				node.Draw(_spriteBatch, Color.Green);
			}
			else
			{
				node.Draw(_spriteBatch);
			}
		}
		DrawWalls(_spriteBatch);
		_player.Draw(_spriteBatch);
		_enemy.Draw(_spriteBatch);
		_enemy.Draw(_spriteBatch, Color.Red);
		_spriteBatch.End();
		base.Draw(gameTime);
	}

	private static void DisplayFrames(GameTime gameTime)
	{
		float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
		Console.Write("Frames: ");
		Console.WriteLine(frameRate);
	}

	private void MoveInput()
	{
		bool up = Keyboard.GetState().IsKeyDown(Keys.W);
		bool down = Keyboard.GetState().IsKeyDown(Keys.S);
		bool left = Keyboard.GetState().IsKeyDown(Keys.A);
		bool right = Keyboard.GetState().IsKeyDown(Keys.D);

		if ((up || down) && !(up && down))
		{
			_inputAxis.Y = Keyboard.GetState().IsKeyDown(Keys.S) ? 1 : -1;
		}
		else
		{
			_inputAxis.Y = 0;
		}

		if ((left || right) && !(left && right))
		{
			_inputAxis.X = Keyboard.GetState().IsKeyDown(Keys.A) ? -1 : 1;
		}
		else
		{
			_inputAxis.X = 0;
		}
	}

	private void AttackInput() {
		if (Mouse.GetState().LeftButton == ButtonState.Pressed) {
			_player.StartAttack();
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

	private static void DrawWalls(SpriteBatch spriteBatch)
	{
		foreach (StaticObject wall in _walls)
		{
			wall.Draw(spriteBatch);
		}
	}
}
