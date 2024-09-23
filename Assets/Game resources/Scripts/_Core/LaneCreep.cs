using UnityEngine;

public class LaneCreep : Creep
{
	private Transform[] _waypoints;
	private bool _pathIsFinished;
	private int _pathIndex;

	public void SetWaypoints(Transform[] waypoints)
	{
		_waypoints = waypoints;
	}

	protected override Vector3 GetTarget()
	{
		_agent.stoppingDistance = 0f;

		if (_closestEnemy != null)
		{
			_agent.stoppingDistance = _attackDistance;
			return _closestEnemy.transform.position;
		}
		else if (_pathIsFinished == false)
		{
			var next = _waypoints[_pathIndex];

			var a = transform.position.SetY(0f);
			var b = next.position.SetY(0f);

			var distance = (a - b).magnitude;
			if (distance < 0.5f)
			{
				_pathIndex++;
				if (_pathIndex == _waypoints.Length)
				{
					_pathIsFinished = true;
				}
			}

			return next.position;
		}

		return transform.position;
	}

	protected override bool IsTargetValid(Target target)
	{
		return target is not NeutralCreep;
	}
}
