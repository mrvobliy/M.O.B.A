using UnityEngine;

public class Projectile : MonoBehaviour
{
	private const float DistanceDetonation = 0.2f;
	
	[SerializeField] private ProjectileVFXControl _projectileVFX;
	
	private EntityComponentsData _tower;
	private EntityComponentsData _target;
	private Transform _followPoint;
	
	private int _damage;
	private float _speed;

	public void Init(EntityComponentsData tower, int damage, EntityComponentsData target, float speed)
	{
		_tower = tower;
		_damage = damage;
		_speed = speed;
		_target = target;
		_followPoint = _target.EntityHealthControl.EnemyAttackPoint;
	}

	private void Update()
	{
		transform.position = Vector3.MoveTowards(transform.position,
			_followPoint.position, _speed * Time.deltaTime);

		transform.LookAt(_followPoint.transform);

		var sqrDistance = (transform.position - _followPoint.position).sqrMagnitude;

		if (sqrDistance >= DistanceDetonation) return;
		
		_target.EntityHealthControl.TakeDamage(_tower, _damage);
		_projectileVFX.SpawnHitEffect(_followPoint);
		Destroy(gameObject);
	}
}