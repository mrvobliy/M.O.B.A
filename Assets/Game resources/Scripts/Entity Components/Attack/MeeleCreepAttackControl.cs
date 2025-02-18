using UnityEngine;

public class MeeleCreepAttackControl : EntityAttackControl
{
    [SerializeField] protected IntVariable _baseDamage;
    
    protected bool _insideAttack;
    
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

    protected override void TryStartAttack()
    {
        if (ClosestEnemyInAttackArea.Count <= 0) return;
        
        if (_insideAttack) return;

        _componentsData.Animator.SetTrigger(AnimatorHash.Attack);
        _insideAttack = true;
    }
    
    private void OnAttackAnimHit()
    {
        if (ClosestEnemyInAttackArea.Count <= 0) return;
        
        ClosestEnemyInAttackArea[0].EntityHealthControl.TakeDamage(_componentsData, _baseDamage.Value);
    }
    
    private void OnAttackEnd() => _insideAttack = false;
}