using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
	[SerializeField] private Transform _rotationParent;
    [SerializeField] private Joystick _joystick;
	[SerializeField] private float _destinationScale;

	private void Awake()
	{
		_agent.updateRotation = false;
	}

	private void Update()
    {
		var inputDirection = new Vector3(_joystick.Direction.x, 0.0f, _joystick.Direction.y);

		if (inputDirection.magnitude <= 0.01f)
		{
			_agent.SetDestination(transform.position);
			VariableBase.IsRunState = false;
			return;
		}

		VariableBase.IsRunState = true;

		_agent.SetDestination(transform.position + inputDirection.normalized * _destinationScale);

		_rotationParent.rotation = Quaternion.LookRotation(inputDirection, Vector3.up);
	}
}
