using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _speedChangeStates;

    private bool _insideAttack;
    private int _damage;
    private AttackTarget _attackTarget;
    private bool _insideIdle = true;

    public void SetToRun()
    {
        if (_insideIdle == false) return;
        _insideIdle = false;

        _animator.DOLayerWeight(1, 1f, 0.3f);
    }

    public void SetToIdle()
    {
		if (_insideIdle) return;
        _insideIdle = true;

		_animator.DOLayerWeight(1, 0f, 0.3f);
    }

    public void TryPlayAttackAnim(AttackTarget target, int damage)
    {
        if (_insideAttack) return;

		_insideAttack = true;
        _attackTarget = target;
        _damage = damage;
        var indexAnim = Random.Range(0, 3);
        _animator.SetTrigger("AttackTrigger_" + indexAnim);
    }

    public void TryDealDamage()
    {
        if (_insideAttack == false) return;

        _attackTarget.TakeDamage(_damage);
    }

	[ContextMenu("TryCompleteAttack")]

	public void TryCompleteAttack()
    {
        if (_insideAttack == false) return;

		_insideAttack = false;
        _attackTarget = null;
        _damage = 0;
	}
}
