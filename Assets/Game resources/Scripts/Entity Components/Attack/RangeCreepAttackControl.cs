using UnityEngine;

public class RangeCreepAttackControl : EntityAttackControl
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _leftProjectileOrigin;
    [SerializeField] private Transform _rightProjectileOrigin;
    [SerializeField] protected IntVariable _damage;
    [SerializeField] private float _projectileSpeed = 1f;
    
    private bool _insideAttack;
    private int _indexAttackAnim;
    
    private void OnEnable()
    {
        _animationEvents.OnAttackBegin += FireProjectile;
        _animationEvents.OnAttackEnd += OnAttackEnd;
    }

    private void OnDisable()
    {
        _animationEvents.OnAttackBegin -= FireProjectile;
        _animationEvents.OnAttackEnd -= OnAttackEnd;
    }

    protected override void TryStartAttack()
    {
        if (ClosestEnemyInAttackArea.Count <= 0) return;
        
        if (_insideAttack) return;
        
        _insideAttack = true;
        _indexAttackAnim++;

        if (_indexAttackAnim > 1)
            _indexAttackAnim = 0;
		
        _componentsData.Animator.SetTrigger(AnimatorHash.GetAttackHash(_indexAttackAnim));
    }
    
    private void FireProjectile()
    {
        var closestEnemyInAttackArea = GetClosestEnemyInAttackArea();
        
        if (closestEnemyInAttackArea == null) return;

        var projectileOrigin = _indexAttackAnim == 1 ? _rightProjectileOrigin : _leftProjectileOrigin;
        var projectile = Instantiate(_projectilePrefab, projectileOrigin.position, projectileOrigin.rotation);

        projectile.Init(_componentsData, _damage.Value, closestEnemyInAttackArea, _projectileSpeed);
    }
    
    private void OnAttackEnd() => _insideAttack = false;
}
