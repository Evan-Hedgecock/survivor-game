using Microsoft.Xna.Framework;
using Core.Objects;
using Core.Physics;
using Core.Systems;
using System.Collections.Generic;
using System;

namespace Core.Entity;
public class Enemy(Rectangle bounds, Grid grid) : Character(bounds) {
    private Pathfinder _pathfinder;
    private List<Vector2> _path;
    private readonly Grid _grid = grid;

    public void Initialize() {
        // Movement properties
        Acceleration = 200;
        Friction = 50;
        Velocity = new Vector2(0, 0);
        MaxSpeed = 50;
        // Positional properties
        CollisionBoxHeight = Bounds.Height / 3;
        CollisionBoxY = CollisionBoxY + Bounds.Height - CollisionBox.Height;
        PositionX = BoundsX;
        PositionY = BoundsY;

        // Health and damage properties
        Health = 100;
        Damage = 10; 

        // Services
        _pathfinder = Global.Services.GetService(typeof(Pathfinder)) as Pathfinder;
        _damageManager = Global.Services.GetService(typeof(DamageManager)) as DamageManager;
        InitializeCollisionManager();
    }

    public void Update(Player player, GameTime gameTime) {
        Node target = _grid.WorldPosToNode(new Vector2(player.CollisionBoxX,
                                                       player.CollisionBoxY));
        Node start = _grid.WorldPosToNode(new Vector2(CollisionBoxX,
                                                      CollisionBoxY));
        _path = _pathfinder.FindPath(start, target);
        MoveAndSlide(MoveDirection(player), gameTime);
        if (_damageManager.IsDamaging(this)) {
            Console.WriteLine("Is damaging");
        } else {
            Console.WriteLine("Is not damaging");
        }
    }

    private Vector2 MoveDirection(Player player) {
        Vector2 direction;
        try {
            direction = Vector2.Subtract(_path[1], _path[0]);
        } catch (Exception) {
            direction = Vector2.Subtract(player.Position, Position);
        }
        return direction;
    }
}