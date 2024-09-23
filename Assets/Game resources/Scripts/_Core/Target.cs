using UnityEngine;
using System;
using DG.Tweening;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[SelectionBase]
public abstract class Target : MonoBehaviour
{
	public static event Action<Target> OnStart;

	public event Action OnDeath;
	public event Action OnDamageTaken;

	[SerializeField] protected Animator _animator;
	[SerializeField] protected Team _team;
	[SerializeField] private float _maxHealth = 100;
	[SerializeField] private bool _useDive = true;
	[SerializeField] private float _diveDelay = 3f;
	[SerializeField] private float _diveDuration = 10f;
	[SerializeField] private float _diveDepth = 1f;
	[SerializeField] private float _regeneration;
	[SerializeField] private Transform[] _safeSpots;

	private List<Transform> _safeSpotsPool = new();

	private float _currentHealth;

	public bool IsBeingAttacked { get; set; }

	public Team Team => _team;
	public float CurrentHealth => _currentHealth;
	public float MaxHealth => _maxHealth;
	public bool IsDead => _currentHealth < 0f || Mathf.Approximately(_currentHealth, 0f);
	public float HealthPercent => _currentHealth / _maxHealth;

	public abstract float Radius { get; }

	protected void Awake()
	{
		_currentHealth = _maxHealth;

		_safeSpotsPool.AddRange(_safeSpots);
	}

	private void Start()
	{
		OnStart?.Invoke(this);
	}

	protected void Update()
	{
		if (IsDead) return;

		_currentHealth = Mathf.MoveTowards(_currentHealth, _maxHealth, Time.deltaTime * _regeneration);
	}

	public void TakeDamage(int damage)
	{
		if (IsDead) return;

		_currentHealth -= damage;

		if (IsDead)
		{
			_currentHealth = 0f;
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

	public Transform GetUnassignedClosestSafeSpot()
	{
		if (_safeSpotsPool.Count == 0)
		{
			return null;
		}

		var safeSpot = _safeSpotsPool.OrderBy(x => DistanceTo(x)).First();
		_safeSpotsPool.Remove(safeSpot);
		return safeSpot;
	}

	public void ReturnSafeSpot(Transform safeSpot)
	{
		_safeSpotsPool.Add(safeSpot);
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