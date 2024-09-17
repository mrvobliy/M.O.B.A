using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
	[SerializeField] private PlayerAnimator _animator;
	[SerializeField] private AttackTarget _me;
    [SerializeField] private int _damage;
	[SerializeField] private float _radius;
	[SerializeField] private Vector3 _center;

	private Collider[] _results = new Collider[64];

	private void FixedUpdate()
	{
		var position = transform.position + _center;

		var target = Helper.FindClosestTarget(position, _radius, _results, _me.Team);

		if (target != null)
		{
			_animator.TryPlayAttackAnim(target, _damage);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position + _center, _radius);
	}
}
