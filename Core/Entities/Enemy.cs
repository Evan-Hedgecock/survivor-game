using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core.Entities;
using Core.Systems;
using Core.Utils;

namespace Core.Entities;
public class Enemy : Actor {

	protected float _findPathTimer = 0.6f;
	protected Player _player;

	public Enemy(Pathfinder pathfinder) {
		_collisionBoxHeight = 8;
		_height = 40;
		_width = 30;
		Body = new Rectangle(200, 1000, _width, _height);
		CollisionBox = new Rectangle(_body.X,
									 _body.Y + _height - _collisionBoxHeight,
									 _body.Width, _body.Height);
		_scale = 0.12f;
		Speed = 2;
		Pfinder = pathfinder;
	}

	public void Update(Player player, Timer findPathTimer) {
		if (!findPathTimer.IsActive()) {
			findPathTimer.Start();
		}
		_player = player;
		_collisionBox.X = Body.X;
		_collisionBox.Y = Body.Y + _height - _collisionBoxHeight;
		ProcessMovement();
	}

	public virtual void GetPath() {
		//Console.WriteLine("Getting path");
		Pfinder.FindTargetCell(_player.CollisionBox);
		Pfinder.FindStartCell(_collisionBox);
		List<Vector2> newPath = new List<Vector2>(Pfinder.FindPath());
		try {
			//Console.WriteLine("New path and old path [0]");
			//Console.WriteLine(_path[0]);
			newPath[0] = _path[0];
			_path = newPath;
			//Console.WriteLine(_path[0]);
		} catch (Exception) {
			//Console.WriteLine("setting path to full new path");
			_path = new List<Vector2>(newPath);
		}
	}

	public override void Draw(SpriteBatch spriteBatch) {
		spriteBatch.Draw(Texture, Body, Color.Red);
	}

	protected void ProcessMovement(Vector2 direction) {
	}

	protected void ProcessMovement() {
		Console.WriteLine("Enemy is processing movement");
		Vector2 direction;
		try {
			direction = new Vector2(_path[0].X - CollisionBox.X,
									_path[0].Y - CollisionBox.Y);
		}
		catch (Exception) {
			direction = new Vector2(0, 0);
		}
  		if (direction.X > 0) {
  			_body.X += Speed;
  		}
  		else if (direction.X < 0) {
  			_body.X -= Speed;
  		}
  
  		if (direction.Y > 0) {
  			_body.Y += Speed;
  		}
  		else if (direction.Y < 0) {
  			_body.Y -= Speed;
 		}
 		try {
 			if (_body.Contains(_path[0])) {
 				Console.Write("Removing path[0]: ");
 				Console.WriteLine(_path[0]);
 				_path.RemoveAt(0);
 			}
 		} catch (Exception) {
 			return;
 		}
 		Console.WriteLine("Lerp of path[0] and body current position: ");
 		Console.WriteLine(Vector2.Lerp(_path[0], new Vector2(_body.X, _body.Y), 0.5f));
	}

	public Timer FindPathTimer() {
		Action cb = GetPath;
		Timer timer = new Timer(_findPathTimer, cb);
		return timer;
	}
}
