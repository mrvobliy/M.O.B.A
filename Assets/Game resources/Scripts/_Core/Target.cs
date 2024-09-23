using UnityEngine;
using System;
using DG.Tweening;
using UnityEditor;

[SelectionBase]
public abstract class Target : MonoBehaviour
{
	public static event Action<Target> OnStart;

	public event Action OnDeath;
	public event Action OnDamageTaken;

	[SerializeField] protected Animator _animator;
	[SerializeField] protected Team _team;
	[SerializeField] private int _maxHealth = 100;
	[SerializeField] private bool _useDive = true;
	[SerializeField] private float _diveDelay = 3f;
	[SerializeField] private float _diveDuration = 10f;
	[SerializeField] private float _diveDepth = 1f;

	private int _currentHealth;

	public bool IsBeingAttacked { get; set; }

	public Team Team => _team;
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

	public float DistanceTo(Vector3 point)
	{
		return (transform.position.SetY(0f) - point.SetY(0f)).magnitude;
	}

	public float DistanceTo(Transform otherTransform)
	{
		return (transform.position.SetY(0f) - otherTransform.position.SetY(0f)).magnitude;
	}

	public float DistanceTo(Target target)
	{
		var dist = (transform.position.SetY(0f) - target.transform.position.SetY(0f)).magnitude;
		dist -= Radius;
		dist -= target.Radius;
		return dist;
	}

	public Vector3 DirectionTo(Vector3 point)
	{
		return (point.SetY(0f) - transform.position.SetY(0f)).normalized;
	}

	public Vector3 DirectionTo(Transform otherTransform)
	{
		return (otherTransform.position.SetY(0f) - transform.position.SetY(0f)).normalized;
	}

	public Vector3 DirectionTo(Target target)
	{
		return (target.transform.position.SetY(0f) - transform.position.SetY(0f)).normalized;
	}

	public float SqrDistanceTo(Vector3 point)
	{
		return (transform.position.SetY(0f) - point.SetY(0f)).sqrMagnitude;
	}

	public float SqrDistanceTo(Transform otherTransform)
	{
		return (transform.position.SetY(0f) - otherTransform.position.SetY(0f)).sqrMagnitude;
	}

#if UNITY_EDITOR
	protected void OnDrawGizmosSelected()
	{
		Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
		Handles.color = new Color(0f, 1f, 0f, 1f);
		Handles.DrawSolidDisc(transform.position + Vector3.up * 0.1f, Vector3.up, Radius);
	}
#endif
}