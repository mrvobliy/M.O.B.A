public class AIMeleeCreepAttackControl : AICreepAttackControl
{
    public override void TryAttack(EntityComponentsData enemy, bool isSpecificTarget)
    {
        if (_entityComponentsData.IsDead) return;
        
        base.TryAttack(enemy, isSpecificTarget);
        
        if (_insideAttack) return;

        _animator.SetTrigger(AnimatorHash.Attack);
        _insideAttack = true;
    }
    
    protected override void OnAttackAnimHit()
    {
        if (_enemyData == null || _enemyData.IsDead || _entityComponentsData.IsDead) return;
        
        _enemyData.EntityHealthControl.TakeDamage(_entityComponentsData, _baseDamage.Value);
    }
    
    private void OnAttackEnd() => _insideAttack = false;
}