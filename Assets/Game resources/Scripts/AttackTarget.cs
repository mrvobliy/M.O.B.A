using UnityEngine;
using System;
using UnityEngine.AI;

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

	[Header("Fill one of these for radius")]
	[SerializeField] private NavMeshAgent _agent;
	[SerializeField] private NavMeshObstacle _obstacle;

	private int _currentHealth;

	public Team Team => _team;
	public int CurrentHealth => _currentHealth;
	public int MaxHealth => _maxHealth;
	public bool IsDead => _currentHealth == 0;
	public float HealthPercent => _currentHealth / (float)_maxHealth;
	public float Radius => _agent == null ? _obstacle.radius : _agent.radius;

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
			if (_agent != null) _agent.enabled = false;
			if (_obstacle != null) _obstacle.enabled = false;
		}

		OnDamageTaken?.Invoke();
	}
}