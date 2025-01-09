using UnityEngine;

public class TowerAttackControl : EntityAttackControl
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _projectileOrigin;
    [SerializeField] protected IntVariable _damage;
    [SerializeField] private float _projectileSpeed = 1f;
    
    private bool _insideAttack;
    private bool _isAttackAnimPlayed;
    private int _indexAttackAnim = 1;

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
    
    private void FireProjectile()
    {
        var closestEnemyInAttackArea = new EntityComponentsData();
        
        foreach (var enemy in ClosestEnemyInAttackArea)
        {
            if (enemy == null) continue;
            
            closestEnemyInAttackArea = enemy;
            break;
        }
        
        if (closestEnemyInAttackArea == null) return;

        var projectile = Instantiate(_projectilePrefab, _projectileOrigin.position, _projectileOrigin.rotation);

        projectile.Init(_entityComponentsData, _damage.Value, closestEnemyInAttackArea, _projectileSpeed);
    }
    
    private void OnAttackEnd() => _insideAttack = false;
}
