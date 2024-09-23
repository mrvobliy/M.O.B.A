using UnityEngine;

public class DetectionZone : MonoBehaviour
{
	[SerializeField] private float _radius;

	public Collider[] Colliders { get; private set; } = new Collider[64];
	public int NearbyAmount { get; private set; }

	private void FixedUpdate()
	{
		NearbyAmount = Physics.OverlapSphereNonAlloc(transform.position, _radius, Colliders);
	}

	public bool HasLaneCreep(Team team)
	{
		for (var i = 0; i < NearbyAmount; i++)
		{
			if (Colliders[i] == null) continue;

			var isCreep = Colliders[i].TryGetComponent(out LaneCreep creep);

			if (isCreep && creep.Team == team)
			{
				return true;
			}
		}

		return false;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(0f, 1f, 0f, 0.4f);
		Gizmos.DrawSphere(transform.position, _radius);
	}
}
