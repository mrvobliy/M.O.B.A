using UnityEngine;

public class Player : Unit
{
	[Header("Player")]
	[SerializeField] private float _destinationScale;

	protected override Vector3 GetTarget()
	{
		_agent.stoppingDistance = 0f;

		var x = Joystick.Instance.Direction.x;
		var y = Joystick.Instance.Direction.y;
		var inputDirection = new Vector3(x, 0f, y);

		if (inputDirection.magnitude < 0.01f)
		{
			return transform.position;
		}

		return transform.position + inputDirection.normalized * _destinationScale;
	}

	protected override bool IsTargetValid()
	{
		return true;
	}
}
