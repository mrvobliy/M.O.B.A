public class AIMeleeCreepAttackControl : AICreepAttackControl
{
    public override void TryAttack(EntityComponentsData enemy, bool isSpecificTarget)
    {
        if (_componentsData.IsDead) return;
        
        base.TryAttack(enemy, isSpecificTarget);
        
        if (_insideAttack) return;

        _componentsData.Animator.SetTrigger(AnimatorHash.Attack);
        _insideAttack = true;
    }
    
    protected override void OnAttackAnimHit()
    {
        if (_enemyData == null || _enemyData.IsDead || _componentsData.IsDead) return;
        
        _enemyData.EntityHealthControl.TakeDamage(_componentsData, _baseDamage.Value);
    }
    
    private void OnAttackEnd() => _insideAttack = false;
}