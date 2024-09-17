using UnityEngine;
using System.Collections.Generic;

public enum Team
{
	Neutral,
	Light,
	Dark,
}

public class AttackTarget : MonoBehaviour
{
	[SerializeField] private Animator _animator;
	[SerializeField] private Team _team;
	[SerializeField] private int _maxHealth;
	[SerializeField] private int _maxTakeDamageAnimations;

	private int _currentHealth;

	public Team Team => _team;
	public int CurrentHealth => _currentHealth;
	public bool IsDead => _currentHealth == 0;

	private void Awake()
	{
		_currentHealth = _maxHealth;
	}

	public void TakeDamage(int damage)
	{
		if (_currentHealth <= 0) return;

		_currentHealth -= damage;

		if (_currentHealth <= 0)
		{
			_currentHealth = 0;
			if (_animator != null) _animator.SetTrigger("Death");
		}
		else
		{
			//_animator.SetTrigger("TakeDamage" + Random.Range(0, _maxTakeDamageAnimations));
		}
	}
}
