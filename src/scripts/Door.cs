using Godot;

public partial class Door : Node3D, IInteractable
{
	[Export]
	private Area3D frontFacingArea3D;


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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void Interact()
	{
		GD.Print("Door Interacted");
	}


	private void OnFrontArea3DBodyEntered(Node3D body)
	{
		GD.Print("Facing front");

	}

	private void OnFrontArea3DBBodyExited(Node3D body)
	{

	}

}
