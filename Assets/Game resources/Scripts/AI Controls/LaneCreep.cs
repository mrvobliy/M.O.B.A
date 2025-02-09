using System.Collections.Generic;
using UnityEngine;

public class LaneCreep : Creep
{
	private Transform[] _waypoints;
	private bool _pathIsFinished;
	private int _pathIndex;

	public void SetWaypoints(Transform[] waypoints) => _waypoints = waypoints;

	protected override Vector3 GetTarget()
	{
		_agent.stoppingDistance = 0f;

		if (_closestEnemyInVisibility != null)
		{
			_agent.stoppingDistance = _attackDistance;
			return _closestEnemyInVisibility.transform.position;
		}

		if (_pathIsFinished) 
			return transform.position;
		
		var next = _waypoints[_pathIndex];

		if (DistanceTo(next) >= 0.5f) return next.position;
		
		_pathIndex++;
		
		if (_pathIndex == _waypoints.Length) 
			_pathIsFinished = true;

		return next.position;
	}

	protected override bool IsTargetValid(Target target) => target is not NeutralCreep;

	public float GetPathDistance()
	{
		var waypoints = _waypoints;

		var minStart = -1;
		var minEnd = -1;
		var minDistance = float.MaxValue;
		List<float> distances = new();

		for (var i = 0; i < waypoints.Length - 1; i++)
		{
			var start = waypoints[i];
			var end = waypoints[i + 1];
			var midPoint = (start.position + end.position) * 0.5f;

			var distance = DistanceTo(midPoint);
			if (distance < minDistance)
			{
				minDistance = distance;
				minStart = i;
				minEnd = i + 1;
			}

			distances.Add((end.position - start.position).magnitude);
		}

		var distanceBefore = 0f;
		for (var i = 0; i < minStart; i++)
		{
			distanceBefore += distances[i];
		}

		var segmentStart = waypoints[minStart].position.SetY(0f);
		var segmentEnd = waypoints[minEnd].position.SetY(0f);

		var vectorToMyself = transform.position.SetY(0f) - segmentStart;
		var vectorToEnd = segmentEnd - segmentStart;

		var segmentDistance = Vector3.Project(vectorToMyself, vectorToEnd).magnitude;
		return distanceBefore + segmentDistance;
	}
}
