using UnityEngine;

public class Tower : MonoBehaviour
{
	[SerializeField] private AnimationEvents _events;
	[SerializeField] private Animator _animator;
	[SerializeField] private AttackTarget _attackTarget;
	[SerializeField] private int _damage;
	[SerializeField] private float _range;
	[SerializeField] private Projectile _projectilePrefab;
	[SerializeField] private Transform _projectileOrigin;
	[SerializeField] private float _projectileSpeed;

	private Collider[] _results = new Collider[64];

	private AttackTarget _target;

	private void Awake()
	{
		_events.OnFireProjectile += OnFireProjectile;
	}

	private void OnFireProjectile()
	{
		if (_target == null) return;

		var projectile = Instantiate(_projectilePrefab,
			_projectileOrigin.position, Quaternion.identity);

		projectile.Init(_damage, _target, _projectileSpeed);
	}

	private void FixedUpdate()
	{
		_animator.SetBool("IsAttacking", false);

		if (_attackTarget.IsDead) return;

		_target = Helper.FindClosestTarget
				(transform.position, _range, _results, _attackTarget.Team);

		if (_target != null)
		{
			if (_target.Team == Team.Neutral)
			{
				return;
			}

			_animator.SetBool("IsAttacking", true);
		}
	}
}
