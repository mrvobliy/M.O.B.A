using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public enum NpcHeroState
{
	None,
	MoveToSafeSpot,
	WonderAroundSafeSpot,
	PushEnemyBuilding,
	AttackLaneCreeps,
	FollowLane
}

public class NpcHero : Unit
{
	[Header("NPC Hero")]
	[SerializeField] private Lane _lane;
	[SerializeField] private float _blendAttackLayerDuration = 0.3f;
	[SerializeField] private float _randomIdleRadius;
	[SerializeField] private float _fallbackHealthPercent = 0.4f;
	[SerializeField] private float _pushHealthPercent = 0.6f;

	private bool _blendAttack;

	private NpcHeroState _currentState = NpcHeroState.MoveToSafeSpot;
	private Vector3 _sampleOrigin;
	private Vector3 _currentSample;
	private bool _sampleGenerated;
	private Transform _currentSafeSpot;
	private Target _currentBuilding;
	private int _pathIndex = -1;
	private bool _pathIsFinished;

	protected override Vector3 GetTarget()
	{
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

		switch (_currentState)
		{
			case NpcHeroState.MoveToSafeSpot:
			{
				var building = Map.Instance.GetFirstAliveBuilding(_team, _lane);

				if (_currentSafeSpot == null)
				{
					_currentSafeSpot = building.GetUnassignedClosestSafeSpot();
					_currentBuilding = building;
				}

				if (DistanceTo(_currentSafeSpot) < 0.01f)
				{
					_currentState = NpcHeroState.WonderAroundSafeSpot;
					_sampleOrigin = _currentSafeSpot.position;
					return transform.position;
				}

				return _currentSafeSpot.position;
			}

			case NpcHeroState.WonderAroundSafeSpot:
			{
				var myCreepsHere = IsFriendlyLaneCreepNearby();

				if (HealthPercent > _pushHealthPercent && myCreepsHere)
				{
					_sampleGenerated = false;
					_currentBuilding.ReturnSafeSpot(_currentSafeSpot);
					_currentSafeSpot = null;
					_currentBuilding = null;
					_currentState = NpcHeroState.FollowLane;
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
					_currentState = NpcHeroState.MoveToSafeSpot;
					return transform.position;
				}
				if (HealthPercent < _fallbackHealthPercent)
				{
					_currentState = NpcHeroState.MoveToSafeSpot;
					return transform.position;
				}

				if (enemyCreepsHere == false)
				{
					_currentState = NpcHeroState.PushEnemyBuilding;
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
				var enemyCreepsHere = IsEnemyLaneCreepNearby();
				if (myCreepsHere == false)
				{
					_currentState = NpcHeroState.MoveToSafeSpot;
					return transform.position;
				}
				if (HealthPercent < _fallbackHealthPercent)
				{
					_currentState = NpcHeroState.MoveToSafeSpot;
					return transform.position;
				}
				if (enemyCreepsHere)
				{
					_currentState = NpcHeroState.AttackLaneCreeps;
					return transform.position;
				}

				var building = Map.Instance.GetFirstAliveBuilding(_team.GetOpposite(), _lane);
				return building.transform.position;
			}

			case NpcHeroState.FollowLane:
			{
				var enemyCreepsHere = IsEnemyLaneCreepNearby();
				if (enemyCreepsHere)
				{
					_pathIndex = -1;
					_currentState = NpcHeroState.AttackLaneCreeps;
				}

				if (_pathIsFinished)
				{
					return transform.position;
				}

				var waypoints = Map.Instance.GetWaypoints(_team, _lane);
				var minDistance = float.MaxValue;

				if (_pathIndex == -1)
				{
					var i = 0;
					for (; i < waypoints.Length; i++)
					{
						var d = DistanceTo(waypoints[i]);

						if (d < minDistance)
						{
							minDistance = d;
							_pathIndex = i;
						}
					}
				}

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
		}

		return transform.position;
	}

	protected override bool IsTargetValid(Target target)
	{
		switch (_currentState)
		{
			case NpcHeroState.None:
			case NpcHeroState.MoveToSafeSpot:
			case NpcHeroState.WonderAroundSafeSpot:
			case NpcHeroState.FollowLane:
				return false;

			case NpcHeroState.PushEnemyBuilding:
				return target is Tower;

			case NpcHeroState.AttackLaneCreeps:
				return target is LaneCreep;

			default:
				return false;
		}
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

		Handles.Label(transform.position, _currentState.ToString());
	}
#endif
}
