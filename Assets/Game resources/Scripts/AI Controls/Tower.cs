using UnityEngine;
using UnityEngine.AI;

public class Tower : Attacker
{
	[Header("Tower")]
	[SerializeField] private NavMeshObstacle _obstacle;
	[SerializeField] private Rigidbody[] _rigidbodies;

	public override float Radius => _obstacle.radius;

	protected override bool IsTargetValid(Target target)
	{
		return true;
	}

	protected new void Awake()
	{
		base.Awake();
		OnDeath += Die;
	}

	private void Die()
	{
		_animator.enabled = false;

		foreach (var rigidbody in _rigidbodies)
		{
			rigidbody.isKinematic = false;
		}
	}
}
