using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackControl : EntityAttackControl
{
    [SerializeField] protected IntVariable _baseDamage;
    
    public event Action<List<EntityComponentsData>> OnTargetsHit;

    private bool _insideAttack;
    private bool _isAttackAnimPlayed;
    private int _indexAttackAnim = 1;

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

        _animator.SetBool(AnimatorHash.IsAttacking, true);
        _insideAttack = true;

        _indexAttackAnim++;

        if (_indexAttackAnim >= 1)
            _indexAttackAnim = 0;
		
        _animator.SetTrigger(AnimatorHash.GetAttackHash(_indexAttackAnim));
    }

    private void OnAttackAnimHit()
    {
        foreach (var enemy in ClosestEnemyInAttackArea)
        {
            if (enemy == null) continue;
            
            enemy.EntityHealthControl.TakeDamage(_entityComponentsData, _baseDamage.Value);
        }

        OnTargetsHit?.Invoke(ClosestEnemyInAttackArea);
    }
    
    private void OnAttackEnd() => _insideAttack = false;
}
