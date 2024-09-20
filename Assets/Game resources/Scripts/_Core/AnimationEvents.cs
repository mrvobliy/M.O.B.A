using UnityEngine;
using System;

public class AnimationEvents : MonoBehaviour
{
	public event Action OnFireProjectile;
	public event Action OnFireProjectile2;
	public event Action OnAttackBegin;
	public event Action OnAttackEnd;
	public event Action OnDeathCompleted;

	public void FireProjectile()
	{
		OnFireProjectile?.Invoke();
	}

	public void FireProjectile2()
	{
		OnFireProjectile2?.Invoke();
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
}
