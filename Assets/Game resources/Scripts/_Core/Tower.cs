using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;

public class Tower : Attacker
{
	[Header("Tower")]
	[SerializeField] private NavMeshObstacle _obstacle;
	[SerializeField] private Rigidbody[] _rigidbodies;
	[SerializeField] private Transform[] _safeSpots;

	private List<Transform> _safeSpotsPool = new();

	public override float Radius => _obstacle.radius;

	protected override bool IsTargetValid(Target target)
	{
		return true;
	}

	protected new void Awake()
	{
		base.Awake();
		OnDeath += Die;

		_safeSpotsPool.AddRange(_safeSpots);
	}

	private void Die()
	{
		_animator.enabled = false;

		foreach (var rigidbody in _rigidbodies)
		{
			rigidbody.isKinematic = false;
		}
	}

	public Transform GetUnassignedClosestSafeSpot()
	{
		if (_safeSpotsPool.Count == 0)
		{
			return null;
		}

		var safeSpot = _safeSpotsPool.OrderBy(x => DistanceTo(x)).First();
		_safeSpotsPool.Remove(safeSpot);
		return safeSpot;
	}

	public void ReturnSafeSpot(Transform safeSpot)
	{
		_safeSpotsPool.Add(safeSpot);
	}
}
