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
}
