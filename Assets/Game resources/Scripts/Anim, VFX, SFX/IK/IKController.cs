using UnityEngine;

public class IKController : MonoBehaviour
{
    public Animator animator;
    public Transform targetTransform;
    public AvatarIKGoal ikGoal = AvatarIKGoal.LeftHand;
    [Range(0, 1)] public float ikWeight = 1.0f;

    void OnAnimatorIK(int layerIndex)
    {
        if (animator != null && targetTransform != null)
        {
            animator.SetIKPositionWeight(ikGoal, ikWeight);
            animator.SetIKRotationWeight(ikGoal, ikWeight);
            animator.SetIKPosition(ikGoal, targetTransform.position);
            animator.SetIKRotation(ikGoal, targetTransform.rotation);
        }
    }
}