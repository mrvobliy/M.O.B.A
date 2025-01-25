using UnityEngine;

public class AIRangeCreepAttackControl : AICreepAttackControl
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _leftProjectileOrigin;
    [SerializeField] private Transform _rightProjectileOrigin;
    [SerializeField] private float _projectileSpeed = 1f;
    
    private int _indexAttackAnim;
    
    public override void TryAttack(EntityComponentsData enemy, bool isSpecificTarget)
    {
        base.TryAttack(enemy, isSpecificTarget);
        
        if (_insideAttack) return;
        
        _insideAttack = true;
        _indexAttackAnim++;

        if (_indexAttackAnim > 1)
            _indexAttackAnim = 0;
		
        _animator.SetTrigger(AnimatorHash.GetAttackHash(_indexAttackAnim));
    }

    protected override void OnAttackAnimHit() => FireProjectile();

    private void FireProjectile()
    {
        var closestEnemy = _isSpecificTarget ? _enemyData : GetClosestEnemyInAttackArea();
        
        print("_isSpecificTarget " + _isSpecificTarget);
        print("closestEnemy " + closestEnemy);
        
        if (closestEnemy == null) return;

        var projectileOrigin = _indexAttackAnim == 1 ? _rightProjectileOrigin : _leftProjectileOrigin;
        var projectile = Instantiate(_projectilePrefab, projectileOrigin.position, projectileOrigin.rotation);

        projectile.Init(_entityComponentsData, _baseDamage.Value, closestEnemy, _projectileSpeed);
    }
}