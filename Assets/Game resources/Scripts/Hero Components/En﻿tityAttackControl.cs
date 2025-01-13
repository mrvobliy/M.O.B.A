using System;
using System.Collections.Generic;
using System.Linq;
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
    protected List<EntityComponentsData> ClosestEnemyInAttackArea { get; private set; } = new();
    

    private Vector3 Forward => _entityComponentsData.EntityHealthControl.RotationParent == null ? 
        transform.forward : _entityComponentsData.EntityHealthControl.RotationParent.forward;
    
    private bool _isPlayerFound;
    
    protected virtual void FixedUpdate()
    {
        if (_entityComponentsData.EntityHealthControl.IsDead) return;
        
        FindClosestEnemiesInVisibilityArea();
        FindClosestEnemiesInAttackArea();
    }

    public EntityComponentsData GetClosestEnemyInVisibilityArea() => ClosestEnemyInVisibilityArea.Count <= 0 ? null : ClosestEnemyInVisibilityArea[0];

    private void FindClosestEnemiesInVisibilityArea()
    {
        var visibilityColliders = new Collider[64];
        Physics.OverlapSphereNonAlloc(transform.position, _detectionRadius, visibilityColliders);

        var closestEnemies = new List<FoundEnemy>();

        foreach (var collider in visibilityColliders)
        {
            if (collider == null) continue;
            
            var foundTarget = collider.GetComponentInChildren<EntityComponentsData>();
            
            if (foundTarget == null) continue;
            
            //if (!collider.TryGetComponent(out EntityComponentsData foundTarget)) continue;
            
            if (foundTarget.EntityTeam == _entityComponentsData.EntityTeam) continue;
            
            if (foundTarget.EntityTeam == Team.Neutral && !_isCanAttackNeutrals) continue;
            
            if (foundTarget.EntityHealthControl.IsDead) continue;

            if (foundTarget.transform.CompareTag("Player"))
                CallPlayerFound();
            
            var distance = (transform.position.SetY(0f) - foundTarget.transform.position.SetY(0f)).sqrMagnitude;
            
            closestEnemies.Add(new FoundEnemy(foundTarget, distance));
        }

        ClosestEnemyInVisibilityArea.Clear();
        ClosestEnemyInVisibilityArea = closestEnemies.OrderBy(x => x.DistanceToTarget).Select(x => x.EntityComponentsData).ToList();
    }

    private void FindClosestEnemiesInAttackArea()
    {
        var closestEnemies = new List<FoundEnemy>();
        
        foreach (var enemy in ClosestEnemyInVisibilityArea)
        {
            var distance = (enemy.transform.position.SetY(0f) - transform.position.SetY(0f)).magnitude;
            
            if (distance > _attackDistance) continue;

            var direction = (enemy.transform.position.SetY(0f) - transform.position.SetY(0f)).normalized;
            var angle = Vector3.Angle(Forward, direction);
            
            if (angle > _maxAngleAttack) continue;
            
            closestEnemies.Add(new FoundEnemy(enemy, distance));
        }

        ClosestEnemyInAttackArea.Clear();
        ClosestEnemyInAttackArea = closestEnemies.OrderBy(x => x.DistanceToTarget).Select(x => x.EntityComponentsData).ToList();
    }
    
    private void CallPlayerFound()
    {
        if (!_isCanCallPlayerFound) return;
        
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
