using UnityEngine;
using DG.Tweening;

public static class Helper
{
	public static Tween DOLayerWeight(this Animator animator, int layer, float to, float duration)
	{
		return DOTween.To(() => animator.GetLayerWeight(layer),
			x => animator.SetLayerWeight(layer, x), to, duration);
	}

	public static Vector3 SetY(this Vector3 vector, float y)
	{
		vector.y = y;
		return vector;
	}

	public static AttackTarget FindClosestTarget(Vector3 position, float radius, Collider[] results, Team myTeam)
	{
		var amount = Physics.OverlapSphereNonAlloc(position, radius, results);

		var minDistance = float.MaxValue;
		AttackTarget target = null;

		for (var i = 0; i < amount; i++)
		{
			var collider = results[i];
			var found = collider.TryGetComponent(out AttackTarget attackTarget);
			if (found == false) continue;
			if (attackTarget.Team == myTeam) continue;
			if (attackTarget.IsDead) continue;

			var distance = (position.SetY(0f) - attackTarget.transform.position.SetY(0f)).sqrMagnitude;
			if (distance < minDistance)
			{
				minDistance = distance;
				target = attackTarget;
			}
		}

		return target;
	}
}
