using UnityEngine;
using UnityEngine.AI;

public class HeroHealthControl : EntityHealthControl
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private int _deathAnimLayer;
    
    public NavMeshAgent Agent => _agent;

    public void RestoreFullHeath()
    {
        _isDead = false;
        _currentHealth = _maxHealth.Value;
        OnHealthChanged?.Invoke();
    }

    protected override void StartDeath()
    {
        base.StartDeath();
        _agent.enabled = false;
        _animator.SetLayerWeight(_deathAnimLayer, 1);
        _animator.SetBool(AnimatorHash.IsDeath, true);
    }
}