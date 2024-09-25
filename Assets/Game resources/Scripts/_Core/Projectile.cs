using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] private ProjectileVFXControl _projectileVFX;
	
	private int _damage;
	private Target _target;
	private Transform _followPoint;
	private float _speed;

	public void Init(int damage, Target target, float speed)
	{
		_damage = damage;
		_speed = speed;
		_target = target;
		_followPoint = target.EnemyAttackPoint != null ? target.EnemyAttackPoint : target.transform;
	}

	private void Update()
	{
		transform.position = Vector3.MoveTowards(transform.position,
			_followPoint.position, _speed * Time.deltaTime);

		transform.LookAt(_followPoint.transform);

		var sqrDistance = (transform.position - _followPoint.position).sqrMagnitude;

		if (sqrDistance < 0.01f)
		{
			_target.TakeDamage(_damage);
			_projectileVFX.SpawnHitEffect(_followPoint);
			Destroy(gameObject);
		}
	}
}
