using UnityEngine;

public abstract class Attacker : Target
{
	[Header("Attacker")]
	[SerializeField] private AnimationEvents _events;
	[SerializeField] private Projectile _projectilePrefab;
	[SerializeField] private Transform _projectileOrigin;
	[SerializeField] private float _projectileSpeed = 1f;
	[SerializeField] private int _attackAnimationAmount = 1;
	[SerializeField] protected float _attackDistance = 1f;
	[SerializeField] protected float _detectionRadius = 5f;
	[SerializeField] protected int _damage = 10;

	private bool _insideAttack;

	protected Collider[] _results = new Collider[64];

	protected Target _targetToKill;

	protected void Awake()
	{
		base.Awake();

		_events.OnFireProjectile += OnFireProjectile;
		_events.OnAttackBegin += OnAttackBegin;
		_events.OnAttackEnd += OnAttackEnd;
	}

	private void OnFireProjectile()
	{
		if (_targetToKill == null) return;

		var projectile = Instantiate(_projectilePrefab,
			_projectileOrigin.position, Quaternion.identity);

		projectile.Init(_damage, _targetToKill, _projectileSpeed);
	}

	private void OnAttackEnd()
	{
		if (_insideAttack == false) return;
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
		_animator.SetBool("IsAttacking", false);

		if (IsDead) return;

		_targetToKill = Helper.FindClosestTarget
				(transform.position, _detectionRadius, _results, Team);

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
				_animator.SetBool("IsAttacking", true);
				TryToAttack();
			}
		}
	}

	public void TryToAttack()
	{
		if (_insideAttack) return;

		_insideAttack = true;
		var indexAnim = Random.Range(0, _attackAnimationAmount) + 1;
		_animator.SetTrigger("Attack" + indexAnim);
	}
}
