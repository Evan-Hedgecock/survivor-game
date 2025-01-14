using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core.Entities;
using Core.Systems;

namespace Core.Entities;
public class Enemy : Actor {
	protected Player _player;

	protected Pathfinder _pathfinder;
	protected GameGrid _gameGrid;

	protected bool findPath = true;
	protected List<Vector2> _path;

	public Enemy(Pathfinder pathfinder, GameGrid gameGrid) {
		_height = 40;
		_collisionBoxHeight = _height / 5;
		_width = 30;
		Body = new Rectangle(0, 0, _width, _height);
		CollisionBox = new Rectangle(_body.X,
									 _body.Y + _height - _collisionBoxHeight,
									 _body.Width, _body.Height);
		Speed = 2;
		_pathfinder = pathfinder;
		_gameGrid = gameGrid;
	}

	public void Update(Player player) {
		_player = player;
		_collisionBox.X = Body.X;
		_collisionBox.Y = Body.Y + _height - _collisionBoxHeight;
		Node start = _gameGrid.WorldPosToNode(Position);
		Node target = FindUnblockedTarget(_gameGrid.WorldPosToNode(
										  new Vector2(_player.CollisionBox.X,
													  _player.CollisionBox.Y)));
		_path = _pathfinder.FindPath(start, target);
		ProcessMovement();
	}

	public override void Draw(SpriteBatch spriteBatch) {
		spriteBatch.Draw(Texture, Body, Color.Red);
	}

	protected void ProcessMovement(Vector2 direction) {
	}

	protected void ProcessMovement() {
		Vector2 direction;
		try {
			if (Position == _path[0]) {
				_path.Remove(Position);
			}
			direction = Vector2.Subtract(_path[0], Position);
			if (_path.Count < 2) {
				direction = Vector2.Subtract(new Vector2(_player.CollisionBox.X,
														 _player.CollisionBox.Y),
											 new Vector2(CollisionBox.X, CollisionBox.Y));
			}
			if (direction.X < 0) {
				_body.X -= Speed;
			} else if (direction.X > 0) {
				_body.X += Speed;
			}
			if (direction.Y < 0) {
				_body.Y -= Speed;
			} else if (direction.Y > 0) {
				_body.Y += Speed;
			}
		} catch (Exception) {}
	}

	protected Node FindUnblockedTarget(Node target) {
		int increment = 1;
		while (target.Blocked) {
			if (target.Blocked) {
				target = _gameGrid.NodeGrid[target.Row, target.Col + increment];
			}
			if (target.Blocked) {
				target = _gameGrid.NodeGrid[target.Row + increment, target.Col];
			}
			if (target.Blocked) {
				target = _gameGrid.NodeGrid[target.Row, target.Col - increment];
			}
			if (target.Blocked) {
				target = _gameGrid.NodeGrid[target.Row - increment, target.Col];
			}
			if (target.Blocked) {
				target = _gameGrid.NodeGrid[target.Row + increment,
					   						target.Col + increment];
			}
			if (target.Blocked) {
				target = _gameGrid.NodeGrid[target.Row - increment,
					   						target.Col - increment];
			}
			increment++;
			Console.WriteLine("No unblocked targets were found");
		}
		return target;
	}
}
