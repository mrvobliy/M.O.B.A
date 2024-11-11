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
	public static event Action<Healthbar, Target> OnCreateHealthBar;
	
	public event Action OnDeath;
	public event Action OnDamageTaken;
	public event Action<Target, int> OnEnemyAttackUs;

	[SerializeField] private Healthbar _healthbarPrefab;
	[SerializeField] protected AnimationEvents _events;
	[SerializeField] protected Animator _animator;
	[SerializeField] protected Team _team;
	[SerializeField] private FloatVariable _maxHealth;
	[SerializeField] protected Transform _rotationParent;
	[SerializeField] private bool _useDive = true;
	[SerializeField] private float _diveDelay = 3f;
	[SerializeField] private float _diveDuration = 10f;
	[SerializeField] private float _diveDepth = 1f;
	[SerializeField] protected Transform _enemyAttackPoint;
	[SerializeField] private bool _dontCreateHealthBar;
	[SerializeField] private float _regeneration;
	[SerializeField] private Transform[] _safeSpots;
	[SerializeField] protected bool _canRebound = true;

	private const float ReboundTime = 0.12f;
	private const float ReboundForce = 0.4f;

	private List<Transform> _safeSpotsPool = new();

	private float _currentHealth;
	
	protected bool _dontCanWork;
	
	public bool DontCanWork => _dontCanWork;
	public bool DontCreateHealthBar => _dontCreateHealthBar;
	public Team Team => _team;
	public bool IsDead => _currentHealth < 0f || Mathf.Approximately(_currentHealth, 0f);
	public float HealthPercent => _currentHealth / _maxHealth.Value;

	public abstract float Radius { get; }

	protected void Awake()
	{
		_currentHealth = _maxHealth.Value;
		_safeSpotsPool.AddRange(_safeSpots);
	}

	private void OnEnable()
	{
		_events.OnDeathCompleted += Death;
	}

	private void OnDisable()
	{
		_events.OnDeathCompleted -= Death;
	}

	protected void Start()
	{
		OnStart?.Invoke(this);
		
		if (!_dontCreateHealthBar)
			OnCreateHealthBar?.Invoke(_healthbarPrefab, this);
	}

	protected virtual void PauseWork()
	{
		_dontCanWork = true;
	}

	protected virtual void ResumeWork()
	{
		_dontCanWork = false;
	}

	public abstract void TryStun(int percentChanceStun, float timeStun);

	protected void Update()
	{
		if (IsDead) return;

		_currentHealth = Mathf.MoveTowards(_currentHealth, _maxHealth.Value, Time.deltaTime * _regeneration);
	}

	public void TakeDamage(Target target, int damage)
	{
		if (IsDead) return;

		_currentHealth -= damage;
		
		OnDamageTaken?.Invoke();
		OnEnemyAttackUs?.Invoke(target, damage);

		RootRebound(target);

		if (!IsDead) return;
		
		_currentHealth = 0f;
		OnDeath?.Invoke();

		_animator.SetTrigger(AnimatorHash.Death);

		if (!_useDive) return;
			
		var targetPos = transform.localPosition.y - _diveDepth;
		transform.DOLocalMoveY(targetPos, _diveDuration)
			.SetDelay(_diveDelay)
			.SetEase(Ease.Linear)
			.OnComplete(Death);
	}
	
	private void RootRebound(Target target)
	{
		if (!target.transform.CompareTag("Player") || _rotationParent == null || !_canRebound) return;
		
		_rotationParent.DOLocalMove(-_rotationParent.forward * ReboundForce, ReboundTime).OnComplete(() =>
		{
			_rotationParent.DOLocalMove(new Vector3(0, 0, 0), ReboundTime);
		});
	}

	private void Death()
	{
		Destroy(gameObject);
	}

	public Transform GetAttackPoint()
	{
		return _enemyAttackPoint != null ? _enemyAttackPoint : transform;
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