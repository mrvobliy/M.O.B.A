using UnityEngine;

public class Projectile : MonoBehaviour
{
	private const float Acceleration = 10f;

	private int _damage;
	private Target _target;
	private float _speed;

	public void Init(int damage, Target target, float speed)
	{
		_damage = damage;
		_target = target;
		_speed = speed;
	}

	private void Update()
	{
		transform.position = Vector3.MoveTowards(transform.position,
			_target.transform.position, _speed * Time.deltaTime);

		_speed += Acceleration * Time.deltaTime;

		transform.LookAt(_target.transform);

		var sqrDistance = (transform.position - _target.transform.position).sqrMagnitude;

		if (sqrDistance < 0.01f)
		{
			_target.TakeDamage(_damage);
			Destroy(gameObject);
		}
	}
}
