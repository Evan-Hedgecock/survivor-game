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
* abstract bool IsColliding(Rectangle potentialPosition): return true if potentialPosition collides with any collider.

## Character extends PhysicsObject:
### Properties

### Methods
* abstract void ProcessMovement(): Update velocity based on movement inputs, pathfinding, steering, or any other mechanisms

## CollisionManager:
CollisionManager should store all collidable gameObjects, and keep a grid position of each.
It will return data to methods and classes that need to know if and where collisions are happening.
Including the normal vector of the collisionObject, collisionDepth (-1, 5) means it's 1 unit
away from the wall in the x direction, and 5 units depth into the wall in the y direction.
### Properties
* List<GameObject> StaticGameObjects: GameObjects that only need to have their position updated once
* List<GameObject> DynamicGameObjects: GameObjects that will need to have their positions updated.
Static and DynamicGameObjects should be sorted by x+y values.
* LinkedList<GameObject> UpdateQueue: Queue of GameObjects that need to be updated.
* List<List<GameObject>> ObjectGrid: Grid that has much larger cells than GameGrid,
It will store each GameObject from the static and dynamic list in each cell that contains it.
If a gameObject overlaps with multipl cells, it should be in multiple cells

### Methods
* public List<GameObject> GetColliding(Rectangle CollisionBox): Calculuate which cell/s the CollisionBox
is in, and return all GameObjects converted to collisionObjects with the penetration depth, 
distance between the CollisionBox and CollisionObject, and penetration depth.

## CollisionObject extends GameObject
### Properties
* Vector2 Distance: Distance between this and the object colliding with it
* Vector2 Normal: Direction perpendicular to side of CollisionObject that was collided with
* Float penDepth: Amount of overlap between CollisionObject and object colliding with it.

## Character extends PhysicsObject
### Properties
* Rectangle hitbox: Slightly inflated collisionbox, when it overlaps with a hurtbox, it will deal damage to that character
* Rectangle hurtbox: Slightly inflated collisionbox, when a hitbox overlaps with it, take damage
* int health: Amount of health character has.
* int damage: Amount of damage character deals.
### Methods
* public int TakeDamage(int amount): Subtracts health by amount.

## Enemy extends Character
### Properties
* Pathfinder pathfinder: Used to track, and find a path to a target (usually, maybe always, the player).
* List<GameObject> path: Path to follow returned from the pathfinder.
* Grid grid: Grid of nodes to find the start and target nodes for pathfinder.

### Methods
* private Vector2 MoveDirection(Player player): Calculates move direction, either based on the path, or if close enough to the player then towards the player.

## StateMachine
Every class that has different states will have access to an instance of a state machine to handle their states.
Its constructor will take a list of states that the class it's a part of needs, and also the initial state of that class.
It will Update the current state every frame, and enter into and exit from other states when necessary.

## State
Abstract state will have an Enter, Exit, and Update functions.
It will also have property referencing which object it's the state of
### Properties
* string Name: Name of this state for quick reference in StatMachine state dictionary
* T Owner: Reference to the object that has this state
### Methods
* void Enter(): Will perform functionality when initially entering into the state
* void Exit(): Will perform functionality when leaving this state
* void Update(): Will perform any updates needed while in this state

# Damaged extends State
State that will put the owner into an invulnerable cooldown.
It will also perform any other functions the owner needs when initially damaged
## Methods
* void Enter(): Check owner health, if health is <= 0 then owner should die. if not:
Start an invulnerability cooldown, and set owner to invulnerable
* void Exit(): set owner invulnerable to false
* void Update(): Countdown invulnerability cooldown, when cooldown is up, then call Exit()


## Other files:
### Core/Enums.cs
* public enum Layer:
    * Background = 0,
    * Midgroud = 1,
    * Foreground = 2,
    * UI = 3

