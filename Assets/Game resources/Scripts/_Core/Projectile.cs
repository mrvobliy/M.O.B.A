using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] private ProjectileVFXControl _projectileVFX;
	private float _distanceDetonation = 0.2f;
	
	private int _damage;
	private Target _tower;
	private Target _target;
	private Transform _followPoint;
	private float _speed;

	public void Init(Target tower, int damage, Target target, float speed)
	{
		_tower = tower;
		_damage = damage;
		_speed = speed;
		_target = target;
		_followPoint = target.GetAttackPoint();
	}

	private void Update()
	{
		transform.position = Vector3.MoveTowards(transform.position,
			_followPoint.position, _speed * Time.deltaTime);

		transform.LookAt(_followPoint.transform);

		var sqrDistance = (transform.position - _followPoint.position).sqrMagnitude;

		if (sqrDistance < _distanceDetonation)
		{
			_target.TakeDamage(_tower, _damage);
			_projectileVFX.SpawnHitEffect(_followPoint);
			Destroy(gameObject);
		}
	}
}
