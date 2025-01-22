using Microsoft.Xna.Framework;
using Core.Objects;
using Core.Physics;
using Core.Systems;
using System.IO;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Character;
public class Enemy(Rectangle bounds, Grid grid) : PhysicsObject(bounds) {
    private Pathfinder _pathfinder;
    private List<Vector2> _path;
    private readonly Grid _grid = grid;
    public void Initialize() {
        Acceleration = 700;
        Friction = 900;
        Velocity = new Vector2(0, 0);
        MaxSpeed = 100;
        CollisionBoxHeight = Bounds.Height / 3;
        CollisionBoxY = CollisionBoxY + Bounds.Height - CollisionBox.Height;
        PositionX = BoundsX;
        PositionY = BoundsY;
        _collisionManager = Global.Services.GetService(typeof(CollisionManager)) as CollisionManager;
        _pathfinder = Global.Services.GetService(typeof(Pathfinder)) as Pathfinder;
        InitializeCollisionManager();
    }

    public void Update(Player player, GameTime gameTime) {
        Node target = _grid.WorldPosToNode(player.Position);
        Node start = _grid.WorldPosToNode(Position);
        _path = _pathfinder.FindPath(start, target);
    }
}