using Godot;

public partial class Player : CharacterBody3D
{
	[Export]
	public float Speed = 5.0f;
	[Export]
	public float JumpVelocity = 4.5f;

	[Export]
	private Node3D cameraNode;

	[Export]
	private float rotationSpeed;
	[Export]
	private float cameraActualRotationSpeed;

	[Export]
	private float verticalRotationLimit = 80f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	private Vector3 targetRotation;

	[Export]
	private RayCast3D interactionRaycast;

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			targetRotation = new Vector3(
				Mathf.Clamp((-1 * mouseMotion.Relative.Y * rotationSpeed) + targetRotation.X, -verticalRotationLimit, verticalRotationLimit),
				Mathf.Wrap((-1 * mouseMotion.Relative.X * rotationSpeed) + targetRotation.Y, 0f, 360f),
				0
			);
		}

		if (@event.IsActionPressed(staticStrings.INTERACTION))
		{
			if (interactionRaycast.IsColliding())
			{
				Node3D collisionObject = interactionRaycast.GetCollider() as Node3D;

				if (!collisionObject.IsInGroup("Interactable"))
				{
					return;
				}

				if (collisionObject.Owner is IInteractable)
				{
					IInteractable interactionObject = collisionObject.Owner as IInteractable;
					interactionObject.Interact(this);
				}
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed(staticStrings.JUMP) && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector(staticStrings.MOVE_LEFT, staticStrings.MOVE_RIGHT, staticStrings.MOVE_UP, staticStrings.MOVE_DOWN);
		Vector3 direction = (cameraNode.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();

		cameraNode.Rotation = new Vector3(
			Mathf.LerpAngle(cameraNode.Rotation.X, Mathf.DegToRad(targetRotation.X), cameraActualRotationSpeed * (float)delta),
			Mathf.LerpAngle(cameraNode.Rotation.Y, Mathf.DegToRad(targetRotation.Y), cameraActualRotationSpeed * (float)delta),
			0
		);
	}
}
