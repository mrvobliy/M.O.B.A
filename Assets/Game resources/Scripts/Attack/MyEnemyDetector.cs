using UnityEngine;

public class MyEnemyDetector : MonoBehaviour
{
	[SerializeField] private Vector3 _center;
	[SerializeField] private float _radius;
	[SerializeField] private Team _enemyTeam;

	private Collider[] _results = new Collider[64];

	private void FixedUpdate()
	{
		var position = transform.position + _center;
		var amount = Physics.OverlapSphereNonAlloc(position, _radius, _results);

		var minDistance = float.MaxValue;
		AttackTarget target = null;

		for (var i = 0; i < amount; i++)
		{
			var collider = _results[i];
			var attackTarget = collider.GetComponent<AttackTarget>();
			if (attackTarget == null) continue;
			if (attackTarget.Team != _enemyTeam) continue;
			

			var distance = (transform.position - attackTarget.transform.position).sqrMagnitude;
			if (distance < minDistance)
			{
				minDistance = distance;
				target = attackTarget;
			}
		}
	}
}
