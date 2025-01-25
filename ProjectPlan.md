# Survivor Game

# Story
A small village that has been built over an ancient underground city. The city was thought to be abandoned, but recently people have been spotting things in the mines and caves around town. Contact was made and the population of undeground dwellers seemed to be peaceful. The village people welcomed them into their town, and everything seemed fine. Village people started exploring deeper into the caves to find and explore the long forgotten underground city. And then rumors started emerging, the city had been corrupted and was ruled by evil monsters. People who had explored into the caves started disappearing, and the village slowly started declining, being corrupted under the influence of the ancient city.

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
## Properties
* Dictionary States: All of the states this state machine has access to. Key: string. Value: State.
* State currentState: The current state that is being updated
* State initialState: What state should be set as current on intialization
## Methods
* void Initialize(): Creates the dictionary of all states this machine has access to, and sets the
inital state as current
* void Update(): Updates the currentState
* void ChangeState(string newState): if the newState is valid, exit current state,
and enter into new state.


# State
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

# BaseUIElement
Base UI class that all UI elements will inherit from
## Properties
* Rectangle Area: The are that the UI element will occupy. Includes drawing location, width and height
* bool Visible: Whether the UI element should be drawn or not
## Methods
* abstract void Draw(): Will draw all of the textures in the proper order
* abstract void Update(): Perform any updates that might change how, when, or where the UI elemnt appears

# ProgressBar extends BaseUIElement
Any UI element that has a background and foreground showing differences between states.
Ex: Loading bar, health bar, stamina bar
## Properties
* Rectangle foregroundArea: area of bar that shows the progress, should have the same X and Y as
the base class Rectangle Area
* Texture2D backgroundTexture: Texture that will take up the full progress bar area
* Texture2D foregroundTexture: Texture that will take up the full foregroundArea denoting the Progress

# HealthBar extends ProgressBar
Progress bar that displays the health of the owner
# Properties
* Vector2 offset: x and y offset from the owner position to where healthbar should be positioned
* Character owner: owner whose health is being displayed

# Equippable
Class that any object the Player can equip will inherit from.
## Properties
* Character Owner: Which character this object will equip to.
* Vector2 EquipPosition: Where on the character should this object be equipped.
* Vector2 EquipOffset: How this item should be offset so it displays properly in its equipped position.
* Vector2 DrawPosition: Determined from the EquipPosition and EquipOffset, where the top-left corner should draw.
* Texture2D Texture: Texture of the item to be drawn when equipped.
* float Scale: Scale to draw texture at.
## Methods
* void Equip(): Equip this item onto the player. Will position the item appropriately,
unequip any other items that conflict with it, and it will update and draw.
* void Unequip(): No longer update or draw this item.
* void Update(): Update the position relative to player.
* void Draw(): Draw the item.

# Weapon
Equippable to any Character, if a weapon is equipped and active, allows that character to perform attacks.
When Character.Attack() is invoked, it will call the Characters active weapons PerformAttack()
## Properties
* int Damage: Amount of damage to deal when attack connects with enemy.
* float AttackDuration: How long each instance of the attack lasts.
Ex: When the attack is called once, a sword might have a swing that will damage an area.
for longer than a single frame, or a gun might shoot a beam that damages an area for a split second.
* Timer AttackDurationTimer: Timer to keep track of how long the attack should be updating.
* float AttackCooldown: How much time until character can use this weapons attack again.
* Timer AttackCooldownTimer: Timer to keep track of attack cooldown.
* bool AttackReady: Determines whether weapon can actually carry out the attack.
Will be set to false when PerformAttack() is called, and AttackCooldownTimer will reset it to true.
* Rectangle HitBox: Area where the attack will hit. Can be a small projectile, or a large sweeping strike.
* float ChargeTime: How much time from character starting attack, until it deals damage.
* Timer ChargeTimer: Will countdown how long the charge is, and unleash the attack when charged.

## Methods
* void ChargeAttack(): If AttackReady, will perform all logic necessary for charging the attack.
Starting ChargeTimer,
Starting animations,
* void PerformAttack(): Once ChargeTimer finishes, this function will actually perform the attack sequence.
Set AttackReady to false,
Create attack hitbox,
Perform attack animation,
* void UpdateAttack(): Update the attack until the AttackDurationTimer is complete.
A hitbox might move across the screen, or the hitbox might not appear until a projectile lands.

# Characters weapon applicable properties and methods
Character will handle and initiate the weapons methods and use the weapons properties.
For example, when the attack is actually performed. The Character will call the damageManager,
and pass in the active weapon.
The Characters methods will represent what the character is do**ing**, the corresponding weapon methods
will be what the weapon does. Ex: Character.ChargingAttack() {
    ActiveWeapon.ChargeAttack();
}
## Properties
* Vector2 WeaponPosition: Where weapons should be positioned on the player.
This is not the top-left drawing location, it's where the players hand would be holding the weapon.
* List<Weapon> WeaponList: List of available weapons to player.
* Weapon ActiveWeapon: Which weapon from weapon list is actively available and being used by the player.
## Methods
* void ChargingAttack(): Has the weapon charge it's attack, and performs any other necessary
character functionality. Ex: movement slowed or stopped, animations, shields, etc.
* void PerformingAttack(): Has the weapon perform its attack, and performs any other necessary
character functionality. Ex: Dashes, animations, etc.
* void UpdatingAttack(): Most attacks will have longer than single frame durations,
while updating, the damageManager will be called to see if the activeWeapon is dealing damage,
and will watch for events ending, or comboing the attack.

## Other files:
### Core/Enums.cs
* public enum Layer:
    * Background = 0,
    * Midgroud = 1,
    * Foreground = 2,
    * UI = 3

