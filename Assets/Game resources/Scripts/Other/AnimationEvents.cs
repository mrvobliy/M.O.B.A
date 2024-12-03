using UnityEngine;
using System;

public class AnimationEvents : MonoBehaviour
{
	public event Action OnFireProjectileLeft;
	public event Action OnFireProjectileRight;
	public event Action OnAttackBegin;
	public event Action OnAttackEnd;
	public event Action OnDeathCompleted;
	public event Action OnPlayAttackEffect;

	public void FireProjectileLeft()
	{
		OnFireProjectileLeft?.Invoke();
	}

	public void FireProjectileRight()
	{
		OnFireProjectileRight?.Invoke();
	}

	public void AttackBegin()
	{
		OnAttackBegin?.Invoke();
	}

	public void AttackEnd()
	{
		OnAttackEnd?.Invoke();
	}

	public void DeathCompleted()
	{
		OnDeathCompleted?.Invoke();
	}
	
	public void PlayAttackEffect()
	{
		OnPlayAttackEffect?.Invoke();
	}
}
