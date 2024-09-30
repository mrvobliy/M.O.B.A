using UnityEngine;

public static class AnimatorHash
{
    public static readonly int Speed = Animator.StringToHash("Speed");
    public static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    public static readonly int Attack1 = Animator.StringToHash("Attack1");
    public static readonly int Attack2 = Animator.StringToHash("Attack2");
    public static readonly int Attack3 = Animator.StringToHash("Attack3");
    public static readonly int Death = Animator.StringToHash("Death");
    public static readonly int Offset = Animator.StringToHash("Offset");
    public static readonly int IsRunning = Animator.StringToHash("IsRunning");

	public static int GetAttackHash(int index)
    {
        return index switch
        {
            0 => Attack1,
            1 => Attack2,
            2 => Attack3,
        };
    }
}
