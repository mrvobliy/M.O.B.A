using UnityEngine;

public abstract class Attacker : Target
{
	[Header("Attacker")]
	[SerializeField] private AnimationEvents _events;
	[SerializeField] protected Transform _rotationParent;
	[SerializeField] private Projectile _projectilePrefab;
	[SerializeField] private Transform _projectileOrigin;
	[SerializeField] private Transform _projectileOrigin2;
	[SerializeField] private float _projectileSpeed = 1f;
	[SerializeField] private int _attackAnimationAmount = 1;
	[SerializeField] protected float _attackDistance = 1f;
	[SerializeField] protected float _detectionRadius = 5f;
	[SerializeField] protected int _damage = 10;
	[SerializeField] protected float _maxAngleAttack = 180f;
	[SerializeField] private bool _isSequentialAttckAnim;

	private bool _insideAttack;
	private int _indexAttackAnim;

	protected Collider[] _results = new Collider[64];
	protected Target _targetToKill;

	protected void Awake()
	{
		base.Awake();

		_events.OnFireProjectile += OnFireProjectile;
		_events.OnFireProjectile2 += OnFireProjectile2;
		_events.OnAttackBegin += OnAttackBegin;
		_events.OnAttackEnd += OnAttackEnd;

		_indexAttackAnim = _attackAnimationAmount;
	}

	private void Fire(Transform origin)
	{
		if (_targetToKill == null) return;

		var projectile = Instantiate(_projectilePrefab,
			origin.position, origin.rotation);

		projectile.Init(_damage, _targetToKill, _projectileSpeed);
	}

	private void OnFireProjectile2()
	{
		Fire(_projectileOrigin2);
	}

	private void OnFireProjectile()
	{
		Fire(_projectileOrigin);
	}

	private void OnAttackEnd()
	{
		if (!_insideAttack) return;
		
		_insideAttack = false;
	}

	private void OnAttackBegin()
	{
		if (_insideAttack == false) return;
		if (_targetToKill == null) return;

		_targetToKill.TakeDamage(_damage);
	}

	protected abstract bool IsTargetValid();

	private void FixedUpdate()
	{
		_animator.SetBool(AnimatorHash.IsAttacking, false);

		if (IsDead) return;

		_targetToKill = FindClosestTarget();

		if (_targetToKill != null)
		{
			if (IsTargetValid() == false)
			{
				_targetToKill = null;
				return;
			}

			var distanceToTarget = Vector3.Distance
				(_targetToKill.transform.position, transform.position);

			distanceToTarget -= Radius;
			distanceToTarget -= _targetToKill.Radius;

			if (distanceToTarget < _attackDistance)
			{
				_animator.SetBool(AnimatorHash.IsAttacking, true);
				TryToAttack();
			}
		}
	}

	public void TryToAttack()
	{
		if (_insideAttack) return;

		_insideAttack = true;

		if (_isSequentialAttckAnim)
		{
			_indexAttackAnim++;

			if (_indexAttackAnim >= _attackAnimationAmount)
				_indexAttackAnim = 0;
		}
		else
		{
			_indexAttackAnim = Random.Range(0, _attackAnimationAmount);
		}
		
		_animator.SetTrigger(AnimatorHash.GetAttackHash(_indexAttackAnim));
	}

	public Target FindClosestTarget()
	{
		var amount = Physics.OverlapSphereNonAlloc(transform.position, _detectionRadius, _results);

		var minDistance = float.MaxValue;
		Target target = null;

		var forward = _rotationParent == null ? transform.forward : _rotationParent.forward;

		for (var i = 0; i < amount; i++)
		{
			var collider = _results[i];
			var found = collider.TryGetComponent(out Target attackTarget);
			if (found == false) continue;
			if (attackTarget.Team == Team) continue;
			if (attackTarget.IsDead) continue;

			var direction = attackTarget.transform.position.SetY(0f) - transform.position.SetY(0f);
			var angle = Vector3.Angle(forward, direction);
			if (angle > _maxAngleAttack) continue;

			var distance = direction.sqrMagnitude;
			if (distance < minDistance)
			{
				minDistance = distance;
				target = attackTarget;
			}
		}

		return target;
	}
}
