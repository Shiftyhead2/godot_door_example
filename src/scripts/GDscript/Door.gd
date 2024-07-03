extends Interactable

@onready var hingePivot: Node3D = %HingePivot
var objectInFront: bool = false
@export var hingeRotationLimit: float:
	get:
		return deg_to_rad(hingeRotationLimit)
	set(value):
		hingeRotationLimit = value
var opened: bool = false
var tween: Tween

func interact(_obj: Node3D=null):
	if (_obj):
		var dir: Vector3 = (transform.origin - _obj.transform.origin).normalized()
		if (dir.dot( - transform.basis.z) > 0):
			objectInFront = true
		elif (dir.dot( - transform.basis.z) < 0):
			objectInFront = false
	opened = !opened
	open_and_close_door()

func open_and_close_door():
	if (tween):
		tween.kill()
	
	tween = create_tween()

	match opened:
		true:
			match objectInFront:
				true:
					tween.tween_property(hingePivot, "rotation:y", hingeRotationLimit, 0.5)
				false:
					tween.tween_property(hingePivot, "rotation:y", -hingeRotationLimit, 0.5)
		false:
			tween.tween_property(hingePivot, "rotation:y", 0, 0.5)
