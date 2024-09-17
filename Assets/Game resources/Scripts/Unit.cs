using UnityEngine;
using UnityEngine.AI;

public enum Team
{
	Neutral,
	Light,
	Dark,
}

public class Unit : MonoBehaviour
{
	[SerializeField] private NavMeshAgent _agent;
	[SerializeField] private Team _team;
	[SerializeField] private Transform _rotationParent;
	[SerializeField] private bool _useWaypoints;

	private Transform[] _waypoints;
	private int _currentWaypoint;
	private bool _finishedPath;

	public void Init(Transform[] waypoints, Quaternion rotation)
	{
		_waypoints = waypoints;
		_rotationParent.rotation = rotation;
	}

	private void Awake()
	{
		_agent.updateRotation = false;
	}

	private void Update()
	{
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

		_rotationParent.rotation = Quaternion.LookRotation(direction, Vector3.up);
	}
}
