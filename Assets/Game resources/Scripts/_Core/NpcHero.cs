using UnityEngine;
using UnityEngine.AI;

public enum NpcHeroState
{
	Idle,
	MoveToTower,
	IdleRandom,
	PushEnemyTower,
	AttackNearbyCreeps
}

public class NpcHero : Unit
{
	[Header("NPC Hero")]
	[SerializeField] private float _blendAttackLayerDuration = 0.3f;
	[SerializeField] private Lane _lane;
	[SerializeField] private float _randomIdleRadius;
	[SerializeField] private float _fallbackHealthPercent = 0.4f;
	[SerializeField] private float _pushHealthPercent = 0.6f;

	private bool _blendAttack;

	private NpcHeroState _currentState = NpcHeroState.MoveToTower;
	private Vector3 _sampleOrigin;
	private Vector3 _currentSample;
	private bool _sampleGenerated;
	private Transform _currentSafeSpot;
	private Tower _currentTower;

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
			case NpcHeroState.MoveToTower:
			{
				var tower = Map.Instance.GetTower(_team, _lane, 2);

				if (_currentSafeSpot == null)
				{
					_currentSafeSpot = tower.GetUnassignedClosestSafeSpot();
					_currentTower = tower;
				}

				if (DistanceTo(_currentSafeSpot) < 0.01f)
				{
					_currentState = NpcHeroState.IdleRandom;
					_sampleOrigin = _currentSafeSpot.position;
					return transform.position;
				}

				return _currentSafeSpot.position;
			}

			case NpcHeroState.IdleRandom:
			{
				var zone = Map.Instance.GetDetector(_lane);
				var myCreepsHere = zone.HasLaneCreep(Team);
				var enemyCreepsHere = zone.HasLaneCreep(_team.GetOpposite());

				if (HealthPercent > _pushHealthPercent && myCreepsHere && enemyCreepsHere)
				{
					_sampleGenerated = false;
					_currentTower.ReturnSafeSpot(_currentSafeSpot);
					_currentSafeSpot = null;
					_currentTower = null;
					_currentState = NpcHeroState.AttackNearbyCreeps;
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

			case NpcHeroState.AttackNearbyCreeps:
			{
				var zone = Map.Instance.GetDetector(_lane);
				var myCreepsHere = zone.HasLaneCreep(Team);
				var enemyCreepsHere = zone.HasLaneCreep(_team.GetOpposite());
				if (myCreepsHere == false)
				{
					_currentState = NpcHeroState.MoveToTower;
					return transform.position;
				}
				if (enemyCreepsHere == false)
				{
					_currentState = NpcHeroState.PushEnemyTower;
				}

				if (HealthPercent < _fallbackHealthPercent)
				{
					_currentState = NpcHeroState.MoveToTower;
				}

				if (_closestEnemy != null)
				{
					_agent.stoppingDistance = _attackDistance;
					return _closestEnemy.transform.position;
				}

				return transform.position;
			}

			case NpcHeroState.PushEnemyTower:
			{
				var zone = Map.Instance.GetDetector(_lane);
				var myCreepsHere = zone.HasLaneCreep(Team);
				var enemyCreepsHere = zone.HasLaneCreep(_team.GetOpposite());
				if (myCreepsHere == false)
				{
					_currentState = NpcHeroState.MoveToTower;
					return transform.position;
				}
				if (enemyCreepsHere)
				{
					_currentState = NpcHeroState.AttackNearbyCreeps;
					return transform.position;
				}

				var tower = Map.Instance.GetTower(_team.GetOpposite(), _lane, 2);
				return tower.transform.position;
			}
		}

		return transform.position;
	}

	protected override bool IsTargetValid(Target target)
	{
		switch (_currentState)
		{
			case NpcHeroState.Idle:
			case NpcHeroState.MoveToTower:
			case NpcHeroState.IdleRandom:
				return true;

			case NpcHeroState.PushEnemyTower:
				return target is Tower;

			case NpcHeroState.AttackNearbyCreeps:
				return target is LaneCreep;
		}

		return false;
	}

	private bool IsFriendlyCreepNearby()
	{
		for (var i = 0; i < _visibilityAmount; i++)
		{
			if (_visibilityColliders[i] == null) continue;

			var isCreep = _visibilityColliders[i].TryGetComponent(out LaneCreep creep);

			if (isCreep && creep.Team == _team)
			{
				return true;
			}
		}

		return false;
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
	}
#endif
}
