using Godot;

public partial class Door : Node3D, IInteractable
{

	[Export]
	private Node3D hingePivot;

	private bool objectInFront = false;

	[Export]
	private float hingeRotationAngle = 90f;

	private bool opened = false;

	private Tween tween;


	public void Interact(Node3D obj = null)
	{
		if (obj != null)
		{
			Vector3 dir = (Transform.Origin - obj.Transform.Origin).Normalized();
			if (dir.Dot(-Transform.Basis.Z) > 0f)
			{
				objectInFront = true;
			}
			else if (dir.Dot(-Transform.Basis.Z) < 0f)
			{
				objectInFront = false;
			}
		}

		opened = !opened;
		OpenOrCloseDoor();
	}

	//Open and closes the door
	private void OpenOrCloseDoor()
	{
		tween?.Kill();

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
