using UnityEngine;

public static class AnimatorHash
{
    public static readonly int Speed = Animator.StringToHash("Speed");
    public static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    public static readonly int Attack = Animator.StringToHash("Attack");
    public static readonly int Attack1 = Animator.StringToHash("Attack1");
    public static readonly int Attack2 = Animator.StringToHash("Attack2");
    public static readonly int Attack3 = Animator.StringToHash("Attack3");
    public static readonly int DeathTrigger = Animator.StringToHash("Death");
    public static readonly int Offset = Animator.StringToHash("Offset");
    public static readonly int IsRunning = Animator.StringToHash("IsRunning");
    public static readonly int IsFirstSkill = Animator.StringToHash("isFirstSkill");
    public static readonly int IsSecondSkill = Animator.StringToHash("isSecondSkill");
    public static readonly int IsThirdSkill = Animator.StringToHash("isThirdSkill");
    public static readonly int IsStun = Animator.StringToHash("isStun");
    public static readonly int ThroneDeath = Animator.StringToHash("throneDeath");
    public static readonly int IsDeath = Animator.StringToHash("isDeath");


    public static int GetAttackHash(int index)
    {
        return index switch
        {
            0 => Attack1,
            1 => Attack2,
            2 => Attack3,
			_ => Attack1
        };
    }
}
