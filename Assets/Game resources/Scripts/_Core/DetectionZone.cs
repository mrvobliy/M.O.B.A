using UnityEngine;
using UnityEditor;

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

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
		Handles.color = new Color(0f, 1f, 0f, 1f);
		Handles.DrawWireDisc(transform.position + Vector3.up * 0.1f, Vector3.up, _radius, 3f);
	}
#endif
}
