using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class EntityHealthControl : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] protected Transform _rotationParent;
    [SerializeField] private IntVariable _maxHealth;
    [SerializeField] private int _deathAnimLayer;
    [SerializeField] private bool _needRootRebound;
    [SerializeField] private bool _needDestroyAfterDeath;
    
    public event Action OnDeathStart;
    public event Action OnDeathEnd;
    public event Action OnHealthChanged;
    
    public event Action<Target, int> OnEnemyAttackUs;
    public float HealthPercent => _currentHealth / _maxHealth.Value;
    public Transform RotationParent => _rotationParent;
    public Animator Animator => _animator;
    public NavMeshAgent Agent => _agent;
    
    private const float ReboundTime = 0.12f;
    private const float ReboundForce = 0.4f;
    private const float DiveDuration = 1f;
    private const float DiveDelay = 2.5f;
    private const float DiveDepth = 4f;
    
    private bool _isDead;
    private float _currentHealth;

    private void Start() => _currentHealth = _maxHealth.Value;

    public void TakeDamage(Target target, int damage)
    {
        if (_isDead) return;

        RootRebound();
        
        _currentHealth -= damage;
        OnHealthChanged?.Invoke();
        OnEnemyAttackUs?.Invoke(target, damage);
        
        if (!(_currentHealth <= 0) || _isDead) 
            return;
        
        _isDead = true;
        OnDeathStart?.Invoke();
        PlayDeathAnimation();
    }
    
    public void RestoreFullHeath()
    {
        _isDead = false;
        _currentHealth = _maxHealth.Value;
        OnHealthChanged?.Invoke();
    }
    
    private void PlayDeathAnimation()
    {
        var sequence = DOTween.Sequence();
        
        sequence.AppendCallback(() =>
        {
            _agent.enabled = false;
            _animator.SetLayerWeight(_deathAnimLayer, 1);
            _animator.SetBool(AnimatorHash.IsDeath, true);
        });
        sequence.AppendInterval(DiveDelay);
        sequence.Append(transform.DOLocalMoveY(transform.parent.localPosition.y - DiveDepth, DiveDuration));
        sequence.AppendCallback(() =>
        {
            OnDeathEnd?.Invoke();
            
            if (_needDestroyAfterDeath)
                Destroy(transform.parent.gameObject);
        }); 
    }
    
    private void RootRebound()
    {
        if (!_needRootRebound || _rotationParent == null) return;
		
        _rotationParent.DOLocalMove(-_rotationParent.forward * ReboundForce, ReboundTime).OnComplete(() =>
        {
            _rotationParent.DOLocalMove(new Vector3(0, 0, 0), ReboundTime);
        });
    }
}
 