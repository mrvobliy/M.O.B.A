using UnityEngine;

public class OnAttackEvent : MonoBehaviour
{
	[SerializeField] private PlayerAnimator _animator;

	public void Attack()
	{
		_animator.TryDealDamage();
	}
}
