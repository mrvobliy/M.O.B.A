using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEditor;
using System.Collections.Generic;

public abstract class Attacker : Target
{
	[Header("Attacker")]
	[SerializeField] private Projectile _projectilePrefab;
	[SerializeField] private Transform _projectileOriginLeft;
	[SerializeField] private Transform _projectileOriginRight;
	[SerializeField] private float _projectileSpeed = 1f;
	[SerializeField] private int _attackAnimationAmount = 1;
	[SerializeField] protected float _attackDistance = 1f;
	[SerializeField] protected float _detectionRadius = 5f;
	[SerializeField] protected int _damage = 10;
	[SerializeField] protected float _maxAngleAttack = 180f;
	[SerializeField] private bool _isSequentialAttckAnim;
	[SerializeField] private bool _spreadDamageAcrossAttackArea;
	[SerializeField] private bool _isCanCallPlayerFound;

	public event Action<Target> OnTargetHit;
	public event Action OnPlayerFound;
	public event Action OnPlayerLost;

	private bool _isPlayerFound;

	private bool _insideAttack;
	private bool _isAttackAnimPlayed;
	private int _indexAttackAnim;

	protected bool _aggressive = true;

	protected Collider[] _visibilityColliders = new Collider[64];
	protected int _visibilityAmount;

	protected Target _closestEnemyInVisibility;
	protected Target _closestEnemyInAttackArea;

	public Vector3 Forward => _rotationParent == null ? transform.forward : _rotationParent.forward;

	protected new void Awake()
	{
		base.Awake();

		_events.OnFireProjectileLeft += OnFireProjectileLeft;
		_events.OnFireProjectileRight += OnFireProjectileRight;
		_events.OnAttackBegin += OnAttackBegin;
		_events.OnAttackEnd += OnAttackEnd;

		_indexAttackAnim = _attackAnimationAmount;
	}

	private void Fire(Transform origin)
	{
		if (_closestEnemyInAttackArea == null) return;

		var projectile = Instantiate(_projectilePrefab,
			origin.position, origin.rotation);

		projectile.Init(this, _damage, _closestEnemyInAttackArea, _projectileSpeed);
	}

	private void OnFireProjectileRight()
	{
		Fire(_projectileOriginRight);
	}

	private void OnFireProjectileLeft()
	{
		Fire(_projectileOriginLeft);
	}

	private void OnAttackEnd()
	{
		if (!_insideAttack) return;
		
		_insideAttack = false;
		_isAttackAnimPlayed = false;
	}

	private void OnAttackBegin()
	{
		if (!_insideAttack) return;

		_closestEnemyInAttackArea = FindClosestEnemyInVisibilityRadius();
		
		if (_closestEnemyInAttackArea == null) return;
		
		if (_isAttackAnimPlayed) return;

		_isAttackAnimPlayed = true;

		if (_spreadDamageAcrossAttackArea)
		{
			foreach (var enemy in FindAllEnemiesInAttackArea())
			{
				enemy.TakeDamage(this, _damage);
			}
		}
		else
		{
			_closestEnemyInAttackArea.TakeDamage(this, _damage);
		}

		OnTargetHit?.Invoke(_closestEnemyInAttackArea);
	}

	protected abstract bool IsTargetValid(Target target);

	private void FixedUpdate()
	{
		if (_isSkillEnable) return;
		
		_animator.SetBool(AnimatorHash.IsAttacking, false);

		if (IsDead) return;

		_visibilityAmount = Physics.OverlapSphereNonAlloc
			(transform.position, _detectionRadius, _visibilityColliders);

		_closestEnemyInVisibility = FindClosestEnemyInVisibilityRadius();
		_closestEnemyInAttackArea = FindClosestEnemyInAttackArea();

		if (_aggressive && _closestEnemyInAttackArea != null)
		{
			_animator.SetBool(AnimatorHash.IsAttacking, true);
			TryToAttack();
		}
	}

	private void TryCallPlayerFound(Target player)
	{
		if (!_isCanCallPlayerFound) return;
		
		if (player == null && _isPlayerFound)
		{
			_isPlayerFound = false;
			OnPlayerLost?.Invoke();
			return;
		}

		if (player != null && !_isPlayerFound)
		{
			_isPlayerFound = true;
			OnPlayerFound?.Invoke();
		}
	}

	public void TryToAttack()
	{
		if (_insideAttack) return;

		_insideAttack = true;

		if (_isSequentialAttckAnim)
		{
			_indexAttackAnim++;

			if (_indexAttackAnim >= _attackAnimationAmount)
				_indexAttackAnim = 0;
		}
		else
		{
			_indexAttackAnim = Random.Range(0, _attackAnimationAmount);
		}
		
		_animator.SetTrigger(AnimatorHash.GetAttackHash(_indexAttackAnim));
	}

	public Target FindClosestEnemyInVisibilityRadius()
	{
		var minDistance = float.MaxValue;
		Target target = null;
		Target player = null;

		for (var i = 0; i < _visibilityAmount; i++)
		{
			var collider = _visibilityColliders[i];
			if (collider == null) continue;

			var found = collider.TryGetComponent(out Target attackTarget);

			if (found == false) continue;
			if (attackTarget.Team == Team) continue;
			if (attackTarget.IsDead) continue;
			if (IsTargetValid(attackTarget) == false) continue;
			
			if (attackTarget.transform.CompareTag("Player"))
				player = attackTarget;
			
			var distance = SqrDistanceTo(attackTarget.transform);
			if (distance < minDistance)
			{
				minDistance = distance;
				target = attackTarget;
			}
		}
		
		TryCallPlayerFound(player);

		return target;
	}

	public IEnumerable<Target> FindAllEnemiesInAttackArea()
	{
		for (var i = 0; i < _visibilityAmount; i++)
		{
			var collider = _visibilityColliders[i];
			if (collider == null) continue;

			var found = collider.TryGetComponent(out Target attackTarget);

			if (found == false) continue;
			if (attackTarget.Team == Team) continue;
			if (attackTarget.IsDead) continue;
			if (IsTargetValid(attackTarget) == false) continue;

			var distance = DistanceTo(attackTarget.transform);
			if (distance > _attackDistance) continue;

			var direction = DirectionTo(attackTarget.transform);
			var angle = Vector3.Angle(Forward, direction);
			if (angle > _maxAngleAttack) continue;

			yield return attackTarget;
		}
	}

	public Target FindClosestEnemyInAttackArea()
	{
		var minDistance = float.MaxValue;
		Target target = null;

		for (var i = 0; i < _visibilityAmount; i++)
		{
			var collider = _visibilityColliders[i];
			if (collider == null) continue;

			var found = collider.TryGetComponent(out Target attackTarget);

			if (found == false) continue;
			if (attackTarget.Team == Team) continue;
			if (attackTarget.IsDead) continue;
			if (IsTargetValid(attackTarget) == false) continue;

			var distance = DistanceTo(attackTarget.transform);
			if (distance > _attackDistance) continue;

			var direction = DirectionTo(attackTarget.transform);
			var angle = Vector3.Angle(Forward, direction);
			if (angle > _maxAngleAttack) continue;

			if (distance < minDistance)
			{
				minDistance = distance;
				target = attackTarget;
			}
		}

		return target;
	}

	public bool IsFriendlyLaneCreepNearby()
	{
		for (var i = 0; i < _visibilityAmount; i++)
		{
			var collider = _visibilityColliders[i];
			if (collider == null) continue;

			var found = collider.TryGetComponent(out LaneCreep target);

			if (found == false) continue;
			if (target.Team != Team) continue;
			if (target.IsDead) continue;

			return true;
		}

		return false;
	}

	public bool AreFriendlyLaneCreepsReadyToPush(float yourPathDistance)
	{
		for (var i = 0; i < _visibilityAmount; i++)
		{
			var collider = _visibilityColliders[i];
			if (collider == null) continue;

			var found = collider.TryGetComponent(out LaneCreep target);

			if (found == false) continue;
			if (target.Team != Team) continue;
			if (target.IsDead) continue;

			var creepDistance = target.GetPathDistance(out _);
			if (creepDistance > yourPathDistance)
			{
				return true;
			}
		}

		return false;
	}

	public bool IsEnemyLaneCreepNearby()
	{
		for (var i = 0; i < _visibilityAmount; i++)
		{
			var collider = _visibilityColliders[i];
			if (collider == null) continue;

			var found = collider.TryGetComponent(out LaneCreep target);

			if (found == false) continue;
			if (target.Team == Team) continue;
			if (target.IsDead) continue;

			return true;
		}

		return false;
	}

	public bool IsEnemyBuildingNearby()
	{
		for (var i = 0; i < _visibilityAmount; i++)
		{
			var collider = _visibilityColliders[i];
			if (collider == null) continue;

			var isTower = collider.TryGetComponent(out Tower tower);
			var isThrone = collider.TryGetComponent(out Throne throne);
			var found = isTower || isThrone;
			Target target = isTower ? tower : throne;

			if (found == false) continue;
			if (target.Team == Team) continue;
			if (target.IsDead) continue;

			return true;
		}

		return false;
	}

	public bool IsFriendlyBuildingNearby(out Target building)
	{
		building = null;

		for (var i = 0; i < _visibilityAmount; i++)
		{
			var collider = _visibilityColliders[i];
			if (collider == null) continue;

			var isTower = collider.TryGetComponent(out Tower tower);
			var isThrone = collider.TryGetComponent(out Throne throne);
			var found = isTower || isThrone;
			Target target = isTower ? tower : throne;

			if (found == false) continue;
			if (target.Team != Team) continue;
			if (target.IsDead) continue;

			building = target;
			return true;
		}

		return false;
	}

#if UNITY_EDITOR
	protected new void OnDrawGizmosSelected()
	{
		Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
		Handles.color = new Color(1f, 0f, 1f, 0.2f);
		Handles.DrawSolidDisc(transform.position + Vector3.up * 0.1f, Vector3.up, _detectionRadius);

		Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
		Handles.color = new Color(1f, 0f, 0f, 1f);
		Handles.DrawSolidArc(transform.position + Vector3.up * 0.1f,
			Vector3.up, Forward, _maxAngleAttack, _attackDistance);
		Handles.DrawSolidArc(transform.position + Vector3.up * 0.1f,
			Vector3.down, Forward, _maxAngleAttack, _attackDistance);

		base.OnDrawGizmosSelected();
	}
#endif
}
