# Survivor Game

# Classes

## GameObject:
### Properties
* Vector2 Position: position of objects top left corner.
* Rectangle Bounds: bounding box of object, used for collisions.
* Layer Layer: Enum representing which rendering layer group this object is a part of.
* Float Z-index: value representing rendering order, 0.0 will render before 1.0, therefore looking like it's below.
### Methods
* abstract void Update(): perform any update logic during game loop Update step.
* abstract void Draw(): draw object during game loop Draw step.

## PhysicsObject extends GameObject:
### Properties
* Vector2 Velocity: distanceX / deltaTime, distanceY / deltaTime.
* Float Acceleration: rate at which velocity is increased over time.
* Float MaxSpeed: what Velocity.X and Velocity.Y is capped at.

### Methods
* abstract void MoveAndSlide(): move object from current position by velocity, don't stop immediately upon collision if not totally blocked. If velocity has Abs(X) and Abs(Y) > 0, if X direction is not blocked, continue moving in X direction.
* abstract void MoveAndCollide(): move object from current position by velocity, stop immediately upon collision.
* abstract Vector2 CheckCollision(Rectangle potentialPosition): check if the potential position intersects any collidable obstacles, if it does, return the normalized vector of the side that was collided with.

## Character extends PhysicsObject:
### Properties

### Methods
* abstract void ProcessMovement(): Update velocity based on movement inputs, pathfinding, steering, or any other mechanisms

## Other files:
### Core/Enums.cs
* public enum Layer:
    * Background = 0,
    * Midgroud = 1,
    * Foreground = 2,
    * UI = 3

