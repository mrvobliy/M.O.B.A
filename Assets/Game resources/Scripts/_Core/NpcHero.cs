using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using TMPro;
using System.Collections.Generic;

public enum NpcHeroState
{
	None,
	MoveToSafeSpot,
	WonderAroundSafeSpot,
	PushEnemyBuilding,
	AttackLaneCreeps,
	FollowLane,
	FollowLaneBack
}

public class NpcHero : Unit
{
	[Header("NPC Hero")]
	[SerializeField] private Lane _lane;
	[SerializeField] private float _blendAttackLayerDuration = 0.3f;
	[SerializeField] private float _randomIdleRadius;
	[SerializeField] private float _fallbackHealthPercent = 0.4f;
	[SerializeField] private float _pushHealthPercent = 0.6f;
	[SerializeField] private TMP_Text _debugText;

	private bool _blendAttack;


	private NpcHeroState _currentState = NpcHeroState.MoveToSafeSpot;
	private NpcHeroState CurrentState
	{
		get => _currentState;
		set
		{
			print($"Change state from {_currentState} to {value}");
			_currentState = value;
		}
	}

	private Vector3 _sampleOrigin;
	private Vector3 _currentSample;
	private bool _sampleGenerated;
	private Transform _currentSafeSpot;
	private Target _currentBuilding;
	private int _pathIndex = -1;
	private bool _pathIsFinished;

	private void Start()
	{
		var building = Map.Instance.GetFirstAliveBuilding(_team, _lane);

		if (_currentSafeSpot == null)
		{
			_currentSafeSpot = building.GetUnassignedClosestSafeSpot();
			_currentBuilding = building;
		}
	}

	protected override Vector3 GetTarget()
	{
		_debugText.text = CurrentState.ToString();

		if (_agent.velocity.magnitude < 0.1f)
		{
			if (_blendAttack)
			{
				_blendAttack = false;
				_animator.DOLayerWeight(2, 0f, _blendAttackLayerDuration);
				_animator.DOLayerWeight(3, 1f, _blendAttackLayerDuration);
			}
		}
		else
		{
			if (_blendAttack == false)
			{
				_blendAttack = true;
				_animator.DOLayerWeight(2, 1f, _blendAttackLayerDuration);
				_animator.DOLayerWeight(3, 0f, _blendAttackLayerDuration);
			}
		}

		_agent.stoppingDistance = 0f;

		switch (CurrentState)
		{
			case NpcHeroState.MoveToSafeSpot:
			{
				//var closestIndex = GetClosestWaypointIndex();
				//var myCreepsHere = AreFriendlyLaneCreepsReadyToPush(closestIndex);

				//if (myCreepsHere && HealthPercent > _pushHealthPercent)
				//{
				//	CurrentState = NpcHeroState.FollowLane;
				//	_sampleGenerated = false;
				//	_currentBuilding.ReturnSafeSpot(_currentSafeSpot);
				//	_currentSafeSpot = null;
				//	_currentBuilding = null;
				//	return transform.position;
				//}

				if (DistanceTo(_currentSafeSpot) < 0.01f)
				{
					CurrentState = NpcHeroState.WonderAroundSafeSpot;
					_sampleOrigin = _currentSafeSpot.position;
					return transform.position;
				}

				return _currentSafeSpot.position;
			}

			case NpcHeroState.WonderAroundSafeSpot:
			{
				var pathDistance = GetPathDistance(out var _);
				var myCreepsHere = AreFriendlyLaneCreepsReadyToPush(pathDistance);

				if (HealthPercent > _pushHealthPercent && myCreepsHere)
				{
					_sampleGenerated = false;
					_currentBuilding.ReturnSafeSpot(_currentSafeSpot);
					_currentSafeSpot = null;
					_currentBuilding = null;
					CurrentState = NpcHeroState.FollowLane;
					return transform.position;
				}

				if (_sampleGenerated == false || DistanceTo(_currentSample) < 0.01f)
				{
					var positionIsValid = false;

					var i = 0;

					do
					{
						i++;

						if (i >= 100)
						{
							Debug.LogError("Too many iterations");
							_currentSample = _sampleOrigin;
							break;
						}

						var angle = Random.Range(0f, 360f);
						var randomEuler = Quaternion.Euler(0f, angle, 0f);
						_currentSample = _sampleOrigin + randomEuler * Vector3.forward * _randomIdleRadius;
						_sampleGenerated = true;

						positionIsValid = NavMesh.SamplePosition
							(_currentSample, out var hit, 0.1f, NavMesh.AllAreas);

						_currentSample = hit.position;
					}
					while (positionIsValid == false);
				}

				return _currentSample;
			}

			case NpcHeroState.AttackLaneCreeps:
			{
				var myCreepsHere = IsFriendlyLaneCreepNearby();
				var enemyCreepsHere = IsEnemyLaneCreepNearby();
				if (myCreepsHere == false)
				{
					CurrentState = NpcHeroState.FollowLaneBack;
					return transform.position;
				}
				if (HealthPercent < _fallbackHealthPercent)
				{
					CurrentState = NpcHeroState.FollowLaneBack;
					return transform.position;
				}

				if (enemyCreepsHere == false)
				{
					CurrentState = NpcHeroState.FollowLane;
					return transform.position;
				}

				if (_closestEnemy != null)
				{
					_agent.stoppingDistance = _attackDistance;
					return _closestEnemy.transform.position;
				}

				return transform.position;
			}

			case NpcHeroState.PushEnemyBuilding:
			{
				var myCreepsHere = IsFriendlyLaneCreepNearby();
				if (myCreepsHere == false)
				{
					CurrentState = NpcHeroState.FollowLane;
					return transform.position;
				}
				if (HealthPercent < _fallbackHealthPercent)
				{
					CurrentState = NpcHeroState.FollowLane;
					return transform.position;
				}

				var enemyCreepsHere = IsEnemyLaneCreepNearby();
				if (enemyCreepsHere)
				{
					CurrentState = NpcHeroState.FollowLane;
					return transform.position;
				}

				var building = Map.Instance.GetFirstAliveBuilding(_team.GetOpposite(), _lane);
				return building.transform.position;
			}

			case NpcHeroState.FollowLane:
			{
				var pathDistance = GetPathDistance(out var pathIndex);
				var myCreepsHere = AreFriendlyLaneCreepsReadyToPush(pathDistance);

				var notEnoughHealth = HealthPercent < _fallbackHealthPercent;

				if (myCreepsHere == false || notEnoughHealth)
				{
					CurrentState = NpcHeroState.FollowLaneBack;
					_pathIndex = -1;
					return transform.position;
				}

				var enemyCreepsHere = IsEnemyLaneCreepNearby();
				if (enemyCreepsHere)
				{
					_pathIndex = -1;
					CurrentState = NpcHeroState.AttackLaneCreeps;
					return transform.position;
				}

				var enemyBuildingHere = IsEnemyBuildingNearby();
				if (enemyBuildingHere)
				{
					_pathIndex = -1;
					CurrentState = NpcHeroState.PushEnemyBuilding;
					return transform.position;
				}

				if (_pathIsFinished)
				{
					return transform.position;
				}

				if (_pathIndex == -1)
				{
					_pathIndex = pathIndex;
				}

				var waypoints = Map.Instance.GetWaypoints(_team, _lane);

				var next = waypoints[_pathIndex];
				if (DistanceTo(next) < 0.5f)
				{
					_pathIndex++;
					if (_pathIndex == waypoints.Length)
					{
						_pathIsFinished = true;
					}
				}

				return next.position;
			}

			case NpcHeroState.FollowLaneBack:
			{
				var pathDistance = GetPathDistance(out var pathIndex);
				var myCreepsHere = AreFriendlyLaneCreepsReadyToPush(pathDistance);

				if (myCreepsHere && HealthPercent > _pushHealthPercent)
				{
					CurrentState = NpcHeroState.FollowLane;
					_pathIndex = -1;
					return transform.position;
				}

				var friendlyBuildingNearby = IsFriendlyBuildingNearby(out var building);
				if (friendlyBuildingNearby)
				{
					_pathIndex = -1;
					CurrentState = NpcHeroState.MoveToSafeSpot;
					if (_currentSafeSpot == null)
					{
						_currentSafeSpot = building.GetUnassignedClosestSafeSpot();
						_currentBuilding = building;
					}
					return transform.position;
				}

				if (_pathIsFinished)
				{
					return transform.position;
				}

				if (_pathIndex == -1)
				{
					_pathIndex = pathIndex;
				}

				var waypoints = Map.Instance.GetWaypoints(_team, _lane);

				var next = waypoints[_pathIndex];
				if (DistanceTo(next) < 0.5f)
				{
					_pathIndex--;
					if (_pathIndex == -1)
					{
						_pathIsFinished = true;
					}
				}

				return next.position;
			}
		}

		return transform.position;
	}

	protected override bool IsTargetValid(Target target)
	{
		switch (CurrentState)
		{
			case NpcHeroState.None:
			case NpcHeroState.MoveToSafeSpot:
			case NpcHeroState.WonderAroundSafeSpot:
			case NpcHeroState.FollowLane:
			case NpcHeroState.FollowLaneBack:
				return false;

			case NpcHeroState.PushEnemyBuilding:
				return target is Tower;

			case NpcHeroState.AttackLaneCreeps:
				return target is LaneCreep;

			default:
				return false;
		}
	}

	private int GetClosestWaypointIndex()
	{
		var waypoints = Map.Instance.GetWaypoints(_team, _lane);
		var minDistance = float.MaxValue;
		var index = -1;

		for (var i = 0; i < waypoints.Length; i++)
		{
			var d = DistanceTo(waypoints[i]);

			if (d < minDistance)
			{
				minDistance = d;
				index = i;
			}
		}

		return index;
	}

	private float GetPathDistance(out int prefferedWaypointIndex)
	{
		var waypoints = Map.Instance.GetWaypoints(_team, _lane);

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
				distances.Add((end.position - start.position).magnitude);
			}
		}

		prefferedWaypointIndex = minEnd;

		var distanceBefore = 0f;
		for (var i = 0; i < minStart; i++)
		{
			distanceBefore += distances[i];
		}

		var segmentStart = waypoints[minStart].position.SetY(0f);
		var segmentEnd = waypoints[minEnd].position.SetY(0f);

		var vectorToMyself = transform.position.SetY(0f) - segmentStart;
		var vectorToEnd = segmentEnd - segmentStart;

		var segmentDistance = Vector3.Dot(vectorToEnd, vectorToMyself);
		return distanceBefore + segmentDistance;
	}

#if UNITY_EDITOR
	protected new void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();

		if (_sampleGenerated)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(_sampleOrigin, 0.1f);

			Gizmos.color = Color.blue;
			Gizmos.DrawCube(_currentSample, Vector3.one * 0.1f);
		}

		Handles.Label(transform.position, CurrentState.ToString());
	}
#endif
}
