using UnityEngine;

public class OnAnimEndEvent : MonoBehaviour
{
    [SerializeField] private PlayerAnimator _animator;

    public void OnAttackAnimEnd()
    {
        _animator.TryCompleteAttack();
    }
}
