using UnityEngine;

public class CreepAttackControl : EntityAttackControl
{
    [SerializeField] protected IntVariable _baseDamage;
    
    private bool _insideAttack;
    
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

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        TryStartAttack();
    }
    
    private void TryStartAttack()
    {
        if (ClosestEnemyInAttackArea.Count <= 0) return;
        
        if (_insideAttack) return;

        _animator.SetTrigger(AnimatorHash.Attack);
        _insideAttack = true;
    }
    
    private void OnAttackAnimHit()
    {
        if (ClosestEnemyInAttackArea.Count <= 0) return;
        
        ClosestEnemyInAttackArea[0].EntityHealthControl.TakeDamage(_entityComponentsData, _baseDamage.Value);
    }
    
    private void OnAttackEnd() => _insideAttack = false;
}