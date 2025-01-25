using UnityEngine;

public class AICreepAttackControl : EntityAttackControl
{
    [SerializeField] protected IntVariable _baseDamage;

    protected bool _insideAttack;
    protected bool _isSpecificTarget;
    protected EntityComponentsData _enemyData;
    
    private void OnEnable()
    {
        _animationEvents.OnAttackBegin += OnAttackAnimHit;
        _animationEvents.OnAttackEnd += OnAttackEnd;
    }

    private void OnDisable()
    {
        _animationEvents.OnAttackBegin -= OnAttackAnimHit;
        _animationEvents.OnAttackEnd -= OnAttackEnd;
    }

    public virtual void TryAttack(EntityComponentsData enemy, bool isSpecificTarget)
    {
        _isSpecificTarget = isSpecificTarget;
        _enemyData = enemy;
    }

    protected virtual void OnAttackAnimHit() {}
    
    private void OnAttackEnd() => _insideAttack = false;
}