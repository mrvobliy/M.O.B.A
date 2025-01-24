using UnityEngine;

public class NeutralCreepMoveControl : EntityMoveControl
{
    private const float OffsetStoppingDistance = 2;
    
    [SerializeField] private float _passiveCooldown;
    [SerializeField] private EntityHealthControl _entityHealthControl;

    public bool IsAggressive { get; private set; } = true;
    
    private EntityComponentsData ClosestEnemyInVisibility =>
        _entityComponentsData.EntityAttackControl.GetClosestEnemyInVisibilityArea();
    
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
            _agent.stoppingDistance = _entityComponentsData.EntityAttackControl.AttackDistance;
            return _spawnPosition;
        }

        if (IsAggressive && ClosestEnemyInVisibility != null)
        {
            _agent.stoppingDistance = _entityComponentsData.EntityAttackControl.AttackDistance / OffsetStoppingDistance;
            return ClosestEnemyInVisibility.transform.position;
        }

        _agent.stoppingDistance = 0f;
        return _spawnPosition;
    }
}