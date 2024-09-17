using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
	[SerializeField] private AttackTarget _attackTarget;
	[SerializeField] private Animator _animator;
	[SerializeField] private NavMeshAgent _agent;
	[SerializeField] private Transform _rotationParent;
	[SerializeField] private bool _useWaypoints;
	[SerializeField] private float _rotationSpeed;

	private Transform[] _waypoints;
	private int _currentWaypoint;
	private bool _finishedPath;

	public void Init(Transform[] waypoints, Quaternion rotation)
	{
		_waypoints = waypoints;
		_rotationParent.rotation = rotation;

		_animator.SetFloat("Offset", Random.Range(0f, 1f));
	}

	private void Awake()
	{
		_agent.updateRotation = false;
	}

	private void Update()
	{
		_animator.SetBool("IsRunning", _agent.velocity.sqrMagnitude > 0.01f);

		if (_attackTarget.IsDead)
		{
			_agent.enabled = false;
			return;
		}

		if (_useWaypoints == false)
		{
			return;
		}

		if (_finishedPath)
		{
			return;
		}

		var target = _waypoints[_currentWaypoint];

		var to = target.position;
		to.y = 0f;
		var from = transform.position;
		from.y = 0f;
		var direction = to - from;

		var sqrDistance = direction.sqrMagnitude;
		if (sqrDistance < 0.1f)
		{
			if (_currentWaypoint == _waypoints.Length - 1)
			{
				_finishedPath = true;
				_agent.SetDestination(transform.position);
				return;
			}

			_currentWaypoint++;
		}

		_agent.SetDestination(target.position);

		var fromRotation = _rotationParent.rotation;
		var toRotation = Quaternion.LookRotation(direction, Vector3.up);
		var speed = Time.deltaTime * _rotationSpeed;
		_rotationParent.rotation = Quaternion.RotateTowards(fromRotation, toRotation, speed);
	}
}
