using UnityEngine;

public class NeutralCreepMoveControl : EntityMoveControl
{
    private const float OffsetStoppingDistance = 2;
    
    [SerializeField] private float _passiveCooldown;
    [SerializeField] private EntityHealthControl _entityHealthControl;

    public bool IsAggressive { get; private set; } = true;
    
    private EntityComponentsData ClosestEnemyInVisibility =>
        _componentsData.EntityAttackControl.GetClosestEnemyInVisibilityArea();
    
    private Vector3 _spawnPosition;
    private float _currentPassiveCooldown;

    private void OnEnable() => _entityHealthControl.OnHealthChanged += ChangeState;
    private void OnDisable() => _entityHealthControl.OnHealthChanged -= ChangeState;

    private void ChangeState()
    {
        _currentPassiveCooldown = _passiveCooldown;
        IsAggressive = true;
    }
    
    protected override Vector3 GetTarget()
    {
        _currentPassiveCooldown = Mathf.MoveTowards(_currentPassiveCooldown, 0f, Time.deltaTime);
        
        if (Mathf.Approximately(_currentPassiveCooldown, 0f))
        {
            IsAggressive = false;
            _componentsData.NavMeshAgent.stoppingDistance = _componentsData.EntityAttackControl.AttackDistance;
            return _spawnPosition;
        }

        if (IsAggressive && ClosestEnemyInVisibility != null)
        {
            _componentsData.NavMeshAgent.stoppingDistance = _componentsData.EntityAttackControl.AttackDistance / OffsetStoppingDistance;
            return ClosestEnemyInVisibility.transform.position;
        }

        _componentsData.NavMeshAgent.stoppingDistance = 0f;
        return _spawnPosition;
    }
}