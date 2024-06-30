using Godot;

public partial class Door : Node3D, IInteractable
{
	[Export]
	private Area3D frontFacingArea3D;

	[Export]
	private Node3D hingePivot;

	private bool objectInFront = false;

	[Export]
	private float hingeRotationAngle = 90f;

	private bool opened = false;

	private Tween tween;


	private CharacterBody3D player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (frontFacingArea3D != null)
		{
			frontFacingArea3D.BodyEntered += OnFrontArea3DBodyEntered;
			frontFacingArea3D.BodyExited += OnFrontArea3DBBodyExited;
		}
	}

	public void Interact()
	{
		opened = !opened;
		OpenOrCloseDoor();
	}


	private void OnFrontArea3DBodyEntered(Node3D body)
	{
		if (body is CharacterBody3D)
		{
			objectInFront = true;
		}
	}

	private void OnFrontArea3DBBodyExited(Node3D body)
	{
		if (body is CharacterBody3D)
		{
			objectInFront = false;
		}
	}

	//Open and closes the door
	private void OpenOrCloseDoor()
	{
		if (tween != null)
		{
			tween.Kill();
		}

		tween = CreateTween();


		switch (opened)
		{
			case false:
				switch (objectInFront)
				{
					case true:
						tween.TweenProperty(hingePivot, "rotation:y", Mathf.DegToRad(hingeRotationAngle), 0.5f);
						break;
					case false:
						tween.TweenProperty(hingePivot, "rotation:y", Mathf.DegToRad(-hingeRotationAngle), 0.5f);
						break;
				}
				break;
			case true:
				tween.TweenProperty(hingePivot, "rotation:y", 0, 0.5f);
				break;
		}
	}



}
