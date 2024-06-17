using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EmeraldAI;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private List<EmeraldAISystem> _allEnemy;
    [SerializeField] private List<EnemyCard> _availableEnemy;
    [SerializeField] private PlayerMovement _playerMovement;
    
    public List<EnemyCard> AvailableEnemy => _availableEnemy;
    
    private EmeraldAISystem _emeraldAI;
    
    private void FixedUpdate()
    {
        //transform.position = _playerMovement.transform.position;
        
        if (_allEnemy.Count <= 0) return;

        _availableEnemy.Clear();
        
        foreach (var enemy in _allEnemy)
        {
            if (enemy.IsDead)
                _allEnemy.Remove(enemy);
            
            if (!RayCast(enemy.AIBoxCollider, out var distance)) continue;
            
            _availableEnemy.Add(new EnemyCard
            {
                AISystem = enemy,
                Distance = distance
            });
        }
        
        if (_availableEnemy.Count <= 0) return;
        
        _availableEnemy = _availableEnemy.OrderBy(t => t.Distance).ToList();
        _playerMovement.SetAttackState(true);
        _playerMovement.RotateToEnemy(_availableEnemy[0].AISystem.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out EmeraldAISystem enemy)) return;
        
        if (enemy.IsDead) return;
        
        if (_allEnemy.Contains(enemy)) return;
        
        _allEnemy.Add(enemy);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out EmeraldAISystem enemy)) return;
        
        if (enemy.IsDead) return;
        
        if (!_allEnemy.Contains(enemy)) return;

        _allEnemy.Remove(enemy);

        if (_allEnemy.Count > 0) return;
        
        _availableEnemy.Clear();
        _playerMovement.SetAttackState(false);
    }
    
    private bool RayCast(Collider other, out float distance)
    {
        var ray = new Ray(transform.position, other.bounds.center - transform.position);
        Physics.Raycast(ray, out var hitInfo, 1000, _layerMask);
        Debug.DrawLine(ray.origin, hitInfo.point, Color.green, 1);
        distance = hitInfo.distance;
        return hitInfo.collider == other;
    }
}

[Serializable]
public class EnemyCard
{
    public EmeraldAISystem AISystem;
    public float Distance;
}
