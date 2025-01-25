using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class NeutralCanAttackCondition : Conditional
{
    [SerializeField] private const float MaxTimeAggressiveState = 10f;
    
    [SerializeField] private EntityAttackControl _entityAttackControl;
    [SerializeField] private EntityHealthControl _entityHealthControl;

    public EntityComponentsData AttackerData { get; private set; }
    
    private bool _isCanStayAggressive;
    private float _currentTimeAggressiveState;
    
    public override void OnStart()
    {
        base.OnStart();
        _entityHealthControl.OnEnemyAttackUs += TryStayAggressive;
    }
    
    public override TaskStatus OnUpdate()
    {
        if (!_isCanStayAggressive)
            return TaskStatus.Failure;

        if (_currentTimeAggressiveState >= MaxTimeAggressiveState) 
            return TaskStatus.Failure;
        
        _currentTimeAggressiveState += Time.deltaTime;

        if (_entityAttackControl.ClosestEnemyInVisibilityArea.Count <= 0)
            return TaskStatus.Failure;
        
        var enemies = _entityAttackControl.ClosestEnemyInVisibilityArea;

        return enemies.Contains(AttackerData) ? TaskStatus.Success : TaskStatus.Failure;
    }

    private void TryStayAggressive(EntityComponentsData attackerData, int damage)
    {
        if (attackerData.EntityType != EntityType.Hero) return;

        _isCanStayAggressive = true;
        AttackerData = attackerData;
    }

    public void ResetState()
    {
        _isCanStayAggressive = false;
        _currentTimeAggressiveState = 0;
    }

    public void ResetAggressiveStateTime() => _currentTimeAggressiveState = 0;
}