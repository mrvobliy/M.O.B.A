using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

public enum UnitBehaviour
{
	Passive,
	Agressive
}

public class Unit : MonoBehaviour
{
	[SerializeField] private AttackTarget _attackTarget;
	[SerializeField] private Animator _animator;
	[SerializeField] private NavMeshAgent _agent;
	[SerializeField] private Transform _rotationParent;
	[SerializeField] private bool _useWaypoints;
	[SerializeField] private float _rotationSpeed;
	[SerializeField] private UnitBehaviour _behaviour;
	[SerializeField] private float _passiveCooldown;
	[SerializeField] private float _searchRadius;
	[SerializeField] private int _damage;
	[SerializeField] private AnimationEvents _events;
	[SerializeField] private float _attackDistance;
	[SerializeField] private float _diveDelay = 3f;
	[SerializeField] private float _diveDuration = 1f;
	[SerializeField] private float _diveDepth = 10f;
	[SerializeField] private Projectile _projectilePrefab;
	[SerializeField] private Transform _projectileOrigin;
	[SerializeField] private float _projectileSpeed;

	private Transform[] _waypoints;
	private int _currentWaypoint;
	private bool _finishedPath;
	private float _currentPassiveCooldown;
	private Vector3 _spawnPosition;

	private Collider[] _results = new Collider[64];

	private AttackTarget _targetToKill;

	public void Init(Transform[] waypoints, Quaternion rotation)
	{
		_waypoints = waypoints;
		_rotationParent.rotation = rotation;

		_animator.SetFloat("Offset", Random.Range(0f, 1f));
	}

	private void Awake()
	{
		_agent.updateRotation = false;

		_spawnPosition = transform.position;

		_attackTarget.OnDamageTaken += OnDamageTaken;
		_events.OnDeathCompleted += OnDeathCompleted;
		_events.OnFireProjectile += OnFireProjectile;
	}

	private void OnFireProjectile()
	{
		if (_targetToKill == null) return;

		var projectile = Instantiate(_projectilePrefab,
			_projectileOrigin.position, Quaternion.identity);

		projectile.Init(_damage, _targetToKill, _projectileSpeed);
	}

	private void OnDeathCompleted()
	{
		var target = transform.localPosition.y - _diveDepth;
		transform.DOLocalMoveY(target, _diveDuration)
			.SetDelay(_diveDelay)
			.SetEase(Ease.Linear)
			.OnComplete(() => Destroy(gameObject));
	}

	private void OnDamageTaken()
	{
		if (_behaviour != UnitBehaviour.Passive) return;
		_currentPassiveCooldown = _passiveCooldown;

		_targetToKill = Helper.FindClosestTarget
			(transform.position, _searchRadius, _results, _attackTarget.Team);
	}

	private void Update()
	{
		_animator.SetBool("IsRunning", _agent.velocity.sqrMagnitude > 0.01f);

		_currentPassiveCooldown = Mathf.MoveTowards(_currentPassiveCooldown, 0f, Time.deltaTime);

		if (_behaviour == UnitBehaviour.Passive &&
			Mathf.Approximately(_currentPassiveCooldown, 0f))
		{
			_targetToKill = null;
		}

		if (_attackTarget.IsDead)
		{
			_agent.enabled = false;
			return;
		}

		if (_useWaypoints == false && _targetToKill == null)
		{
			return;
		}

		if (_finishedPath)
		{
			return;
		}

		var useWaypoints = _targetToKill == null;

		_agent.stoppingDistance = useWaypoints ? 0f : _attackDistance;

		var target = useWaypoints ? _waypoints[_currentWaypoint] : _targetToKill.transform;

		var to = target.position;
		to.y = 0f;
		var from = transform.position;
		from.y = 0f;
		var direction = to - from;

		if (useWaypoints)
		{
			var distance = direction.magnitude;
			if (distance - _agent.stoppingDistance < 0.5f)
			{
				if (_currentWaypoint == _waypoints.Length - 1)
				{
					_finishedPath = true;
					_agent.SetDestination(transform.position);
					return;
				}

				_currentWaypoint++;
			}
		}

		_agent.SetDestination(target.position);

		var fromRotation = _rotationParent.rotation;
		var toRotation = Quaternion.LookRotation(direction, Vector3.up);
		var speed = Time.deltaTime * _rotationSpeed;
		_rotationParent.rotation = Quaternion.RotateTowards(fromRotation, toRotation, speed);
	}

	private void FixedUpdate()
	{
		if (_attackTarget.IsDead) return;

		if (_behaviour == UnitBehaviour.Agressive)
		{
			_targetToKill = Helper.FindClosestTarget
				(transform.position, _searchRadius, _results, _attackTarget.Team);

			if (_targetToKill != null)
			{
				if (_targetToKill.Team == Team.Neutral)
				{
					_targetToKill = null;
					return;
				}

				var distanceToTarget = Vector3.Distance
					(_targetToKill.transform.position, transform.position);

				distanceToTarget -= _agent.radius;
				distanceToTarget -= _targetToKill.Radius;

				if (distanceToTarget < _attackDistance)
				{
					_events.TryToAttack(_targetToKill, _damage);
				}
			}
		}
	}
}
