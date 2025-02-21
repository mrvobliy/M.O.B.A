using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FountainAttackControl : MonoBehaviour
{
    [SerializeField] private EntityComponentsData _componentsData;
    [SerializeField] private List<EntityComponentsData> _enemy;
    [SerializeField] private Team _team;
    [Space]
    [SerializeField] private Transform _projectileOrigin;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] protected IntVariable _damage;
    [SerializeField] private float _projectileSpeed = 1f;
    [SerializeField] private float _timeBetweenAttack;

    private EntityComponentsData _target;
    private Coroutine _onAttackCoroutine;

    private void OnTriggerEnter(Collider collider)
    {
        var componentsData = collider.GetComponentInChildren<EntityComponentsData>();
        
        if (componentsData == null) return;
        
        if (componentsData.EntityTeam == _team) return;
        
        _enemy.Add(componentsData);
        UpdateTarget();

        _onAttackCoroutine ??= StartCoroutine(OnAttack());
    }

    private void OnTriggerExit(Collider collider)
    {
        var componentsData = collider.GetComponentInChildren<EntityComponentsData>();
        
        if (componentsData == null) return;
        
        if (componentsData.EntityTeam == _team) return;

        _enemy.Remove(componentsData);
        UpdateTarget();
    }

    private void UpdateTarget()
    {
        if (!_enemy.Contains(_target)) 
            _target = null;
        
        if (_target != null || _enemy.Count <= 0 || _enemy[0].IsDead) return;

        _target = _enemy[0];
    }

    private IEnumerator OnAttack()
    {
        var delay = new WaitForSeconds(_timeBetweenAttack);

        while (_target != null && !_componentsData.IsDead)
        {
            if (_target.IsDead)
            {
                _target = null;
                UpdateTarget();
                continue;
            }
            
            FireProjectile();
            yield return delay;
        }

        _onAttackCoroutine = null;
    }
    
    private void FireProjectile()
    {
        if (_target.IsDead) return;
        
        var projectile = Instantiate(_projectilePrefab, _projectileOrigin.position, _projectileOrigin.rotation);
        projectile.Init(_componentsData, _damage.Value, _target, _projectileSpeed);
    }
}