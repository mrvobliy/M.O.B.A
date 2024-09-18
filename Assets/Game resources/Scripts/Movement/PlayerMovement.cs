using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private Animator _animator;
	[SerializeField] private PlayerAnimator _playerAnimator;
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
		_animator.SetFloat("Speed", _agent.velocity.magnitude);

		var inputDirection = new Vector3(_joystick.Direction.x, 0.0f, _joystick.Direction.y);

		if (inputDirection.magnitude <= 0.01f)
		{
			_agent.SetDestination(transform.position);
			_playerAnimator.SetToIdle();
			return;
		}

		_playerAnimator.SetToRun();

		_agent.SetDestination(transform.position + inputDirection.normalized * _destinationScale);

		_rotationParent.rotation = Quaternion.LookRotation(inputDirection, Vector3.up);
	}
}
