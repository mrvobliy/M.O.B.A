using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EntityAttackControl : MonoBehaviour
{
    [SerializeField] protected EntityComponentsData _entityComponentsData;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected AnimationEvents _animationEvents;
    [SerializeField] private float _detectionRadius = 5f;
    [SerializeField] protected float _attackDistance = 1f;
    [SerializeField] protected float _maxAngleAttack = 180f;
    [SerializeField] private bool _isCanAttackNeutrals = true;

    private bool IsNeedShowBool => _entityComponentsData.EntityType == EntityType.Tower;
    [SerializeField, ShowIf(nameof(IsNeedShowBool))] private bool _isCanCallPlayerFound;

    public event Action OnPlayerFound;
    public float AttackDistance => _attackDistance;

    public List<EntityComponentsData> ClosestEnemyInVisibilityArea { get; private set; } = new();
    public List<EntityComponentsData> ClosestEnemyInAttackArea { get; private set; } = new();

    private Vector3 Forward => _entityComponentsData.EntityHealthControl.RotationParent == null ?
        transform.forward : _entityComponentsData.EntityHealthControl.RotationParent.forward;

    private bool _isPlayerFound;
    
    private readonly List<FoundEnemy> _visibilityEnemies = new();
    private readonly List<FoundEnemy> _attackEnemies = new();
    private readonly Collider[] _visibilityColliders = new Collider[64];

    protected virtual void FixedUpdate()
    {
        if (_entityComponentsData.EntityHealthControl.IsDead) return;

        if (!_entityComponentsData.CanComponentsWork) return;

        FindClosestEnemiesInVisibilityArea();
        FindClosestEnemiesInAttackArea();
        TryStartAttack();
    }

    public EntityComponentsData GetClosestHeroInVisibilityArea() => ClosestEnemyInVisibilityArea.Find(x => x.EntityType == EntityType.Hero);
    public EntityComponentsData GetClosestLaneCreepInVisibilityArea() => ClosestEnemyInVisibilityArea.Find(x => x.EntityType is EntityType.LaneRange or EntityType.LaneMelee);
    public EntityComponentsData GetClosestEnemyInVisibilityArea() => ClosestEnemyInVisibilityArea.Count > 0 ? ClosestEnemyInVisibilityArea[0] : null;
    public EntityComponentsData GetClosestEnemyInAttackArea() => ClosestEnemyInAttackArea.Count > 0 ? ClosestEnemyInAttackArea[0] : null;

    protected virtual void TryStartAttack() { }

    private void FindClosestEnemiesInVisibilityArea()
    {
        _visibilityEnemies.Clear();
        
        Array.Clear(_visibilityColliders, 0, _visibilityColliders.Length);

        var colliderCount = Physics.OverlapSphereNonAlloc(transform.position, _detectionRadius, _visibilityColliders);

        for (var i = 0; i < colliderCount; i++)
        {
            var collider = _visibilityColliders[i];
            
            if (collider == null) 
                continue;

            var foundTarget = collider.GetComponentInChildren<EntityComponentsData>();
            
            if (foundTarget == null) 
                continue;

            if (foundTarget.EntityTeam == _entityComponentsData.EntityTeam) 
                continue;
            
            if (foundTarget.EntityTeam == Team.Neutral && !_isCanAttackNeutrals) 
                continue;
            
            if (foundTarget.EntityHealthControl.IsDead) 
                continue;

            if (foundTarget.transform.CompareTag("Player"))
                CallPlayerFound();

            var distance = Vector3.SqrMagnitude(transform.parent.position.SetY(0f) - foundTarget.transform.parent.position.SetY(0f));
            _visibilityEnemies.Add(new FoundEnemy(foundTarget, distance));
        }

        _visibilityEnemies.Sort((x, y) => x.DistanceToTarget.CompareTo(y.DistanceToTarget));
        ClosestEnemyInVisibilityArea.Clear();
        
        foreach (var enemy in _visibilityEnemies) 
            ClosestEnemyInVisibilityArea.Add(enemy.EntityComponentsData);
    }

    private void FindClosestEnemiesInAttackArea()
    {
        _attackEnemies.Clear();

        foreach (var enemy in ClosestEnemyInVisibilityArea)
        {
            var distance = Vector3.Distance(enemy.transform.parent.position.SetY(0f), transform.parent.position.SetY(0f));

            if (distance > _attackDistance) 
                continue;

            var direction = (enemy.transform.parent.position.SetY(0f) - transform.parent.position.SetY(0f)).normalized;
            var angle = Vector3.Angle(Forward, direction);

            if (angle > _maxAngleAttack) 
                continue;

            _attackEnemies.Add(new FoundEnemy(enemy, distance));
        }

        _attackEnemies.Sort((x, y) => x.DistanceToTarget.CompareTo(y.DistanceToTarget));
        ClosestEnemyInAttackArea.Clear();
        
        foreach (var enemy in _attackEnemies) 
            ClosestEnemyInAttackArea.Add(enemy.EntityComponentsData);
    }

    private void CallPlayerFound()
    {
        if (!_isCanCallPlayerFound) 
            return;

        OnPlayerFound?.Invoke();
    }
}

public class FoundEnemy
{
    public readonly EntityComponentsData EntityComponentsData;
    public readonly float DistanceToTarget;

    public FoundEnemy(EntityComponentsData entityComponentsData, float distanceToTarget)
    {
        EntityComponentsData = entityComponentsData;
        DistanceToTarget = distanceToTarget;
    }
}