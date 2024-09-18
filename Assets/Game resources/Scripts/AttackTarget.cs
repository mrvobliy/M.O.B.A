using UnityEngine;
using System;

public enum Team
{
	Neutral,
	Light,
	Dark,
}

public class AttackTarget : MonoBehaviour
{
	public static event Action<AttackTarget> OnAwake;

	public event Action OnDeath;
	public event Action OnDamageTaken;

	[SerializeField] private Animator _animator;
	[SerializeField] private Team _team;
	[SerializeField] private int _maxHealth;

	private int _currentHealth;

	public Team Team => _team;
	public int CurrentHealth => _currentHealth;
	public int MaxHealth => _maxHealth;
	public bool IsDead => _currentHealth == 0;
	public float HealthPercent => _currentHealth / (float)_maxHealth;

	private void Awake()
	{
		_currentHealth = _maxHealth;
		OnAwake?.Invoke(this);
	}

	public void TakeDamage(int damage)
	{
		if (_currentHealth <= 0) return;

		_currentHealth -= damage;

		if (_currentHealth <= 0)
		{
			_currentHealth = 0;
			if (_animator != null) _animator.SetTrigger("Death");
			OnDeath?.Invoke();
		}

		OnDamageTaken?.Invoke();
	}
}