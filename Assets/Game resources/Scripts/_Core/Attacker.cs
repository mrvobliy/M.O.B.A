using UnityEngine;
using UnityEditor;

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

	private bool _insideAttack;

	protected Collider[] _visibilityColliders = new Collider[64];
	protected int _visibilityAmount;

	protected Target _closestEnemy;

	public Vector3 Forward => _rotationParent == null ? transform.forward : _rotationParent.forward;

	protected new void Awake()
	{
		base.Awake();

		_events.OnFireProjectile += OnFireProjectile;
		_events.OnFireProjectile2 += OnFireProjectile2;
		_events.OnAttackBegin += OnAttackBegin;
		_events.OnAttackEnd += OnAttackEnd;
	}

	private void Fire(Transform origin)
	{
		if (_closestEnemy == null) return;

		var projectile = Instantiate(_projectilePrefab,
			origin.position, Quaternion.identity);

		projectile.Init(_damage, _closestEnemy, _projectileSpeed);
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
		if (_insideAttack == false) return;
		_insideAttack = false;
	}

	private void OnAttackBegin()
	{
		if (_insideAttack == false) return;
		if (_closestEnemy == null) return;

		_closestEnemy.TakeDamage(_damage);
	}

	protected abstract bool IsTargetValid(Target target);

	private void FixedUpdate()
	{
		_animator.SetBool(AnimatorHash.IsAttacking, false);

		if (_closestEnemy != null)
		{
			_closestEnemy.IsBeingAttacked = false;
		}

		if (IsDead) return;

		_visibilityAmount = Physics.OverlapSphereNonAlloc
			(transform.position, _detectionRadius, _visibilityColliders);

		_closestEnemy = FindClosestTarget();

		if (_closestEnemy != null)
		{
			var distanceToEnemy = DistanceTo(_closestEnemy);
			var direction = DirectionTo(_closestEnemy);
			var angle = Vector3.Angle(Forward, direction);

			if (distanceToEnemy < _attackDistance &&
				angle < _maxAngleAttack)
			{
				_animator.SetBool(AnimatorHash.IsAttacking, true);
				_closestEnemy.IsBeingAttacked = true;
				TryToAttack();
			}
		}
	}

	public void TryToAttack()
	{
		if (_insideAttack) return;

		_insideAttack = true;
		var indexAnim = Random.Range(0, _attackAnimationAmount) + 1;
		_animator.SetTrigger(AnimatorHash.GetAttackHash(indexAnim));
	}

	public Target FindClosestTarget()
	{
		var minDistance = float.MaxValue;
		Target target = null;

		for (var i = 0; i < _visibilityAmount; i++)
		{
			var collider = _visibilityColliders[i];
			var found = collider.TryGetComponent(out Target attackTarget);

			if (found == false) continue;
			if (attackTarget.Team == Team) continue;
			if (attackTarget.IsDead) continue;
			if (IsTargetValid(attackTarget) == false) continue;

			var distance = SqrDistanceTo(attackTarget.transform);
			if (distance < minDistance)
			{
				minDistance = distance;
				target = attackTarget;
			}
		}

		return target;
	}

#if UNITY_EDITOR
	protected new void OnDrawGizmosSelected()
	{
		Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
		Handles.color = new Color(1f, 0f, 1f, 0.2f);
		Handles.DrawSolidDisc(transform.position + Vector3.up * 0.1f, Vector3.up, _detectionRadius);

		Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
		Handles.color = new Color(1f, 0f, 0f, 1f);
		Handles.DrawSolidArc(transform.position + Vector3.up * 0.1f,
			Vector3.up, Forward, _maxAngleAttack, _attackDistance);
		Handles.DrawSolidArc(transform.position + Vector3.up * 0.1f,
			Vector3.down, Forward, _maxAngleAttack, _attackDistance);

		base.OnDrawGizmosSelected();
	}
#endif
}
