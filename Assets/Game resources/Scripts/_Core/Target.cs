using UnityEngine;
using System;
using DG.Tweening;

public enum Team
{
	Neutral,
	Light,
	Dark,
}

public abstract class Target : MonoBehaviour
{
	public static event Action<Target> OnStart;

	public event Action OnDeath;
	public event Action OnDamageTaken;

	[SerializeField] protected Animator _animator;
	[SerializeField] private Team _team;
	[SerializeField] private int _maxHealth = 100;
	[SerializeField] private bool _useDive = true;
	[SerializeField] private float _diveDelay = 3f;
	[SerializeField] private float _diveDuration = 10f;
	[SerializeField] private float _diveDepth = 1f;
	[SerializeField] protected Transform _enemyAttackPoint;
	[SerializeField] private bool _dontCreateHealthBar;

	private int _currentHealth;

	public bool DontCreateHealthBar => _dontCreateHealthBar;
	public Team Team => _team;
	public Transform EnemyAttackPoint => _enemyAttackPoint;
	public int CurrentHealth => _currentHealth;
	public int MaxHealth => _maxHealth;
	public bool IsDead => _currentHealth == 0;
	public float HealthPercent => _currentHealth / (float)_maxHealth;

	public abstract float Radius { get; }

	protected void Awake()
	{
		_currentHealth = _maxHealth;
	}

	private void Start()
	{
		OnStart?.Invoke(this);
	}

	public void TakeDamage(int damage)
	{
		if (_currentHealth <= 0) return;

		_currentHealth -= damage;

		if (_currentHealth <= 0)
		{
			_currentHealth = 0;
			OnDeath?.Invoke();

			_animator.SetTrigger(AnimatorHash.Death);
			if (_useDive)
			{
				var target = transform.localPosition.y - _diveDepth;
				transform.DOLocalMoveY(target, _diveDuration)
					.SetDelay(_diveDelay)
					.SetEase(Ease.Linear)
					.OnComplete(() => Destroy(gameObject));
			}
		}

		OnDamageTaken?.Invoke();
	}
}