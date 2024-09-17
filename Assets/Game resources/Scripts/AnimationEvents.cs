using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
	[SerializeField] private Animator _animator;
	[SerializeField] private int _attackAnimationAmount;
	
	private AttackTarget _target;
	private int _damage;
	private bool _insideAttack;

	public void TryToAttack(AttackTarget target, int damage)
	{
		if (_insideAttack) return;

		_insideAttack = true;
		_target = target;
		_damage = damage;
		var indexAnim = Random.Range(0, _attackAnimationAmount) + 1;
		_animator.SetTrigger("Attack" + indexAnim);
	}

	public void AttackBegin()
	{
		if (_insideAttack == false) return;

		_target.TakeDamage(_damage);
	}

	public void AttackEnd()
	{
		if (_insideAttack == false) return;

		_insideAttack = false;
		_target = null;
		_damage = 0;
	}
}
