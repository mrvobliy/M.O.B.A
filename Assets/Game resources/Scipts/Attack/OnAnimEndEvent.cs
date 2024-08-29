using UnityEngine;

public class OnAnimEndEvent : MonoBehaviour
{
    public void OnAttackAnimEnd()
    {
        PlayerAnimator.Instance.TryPlayAttackAnim();
    }
}
