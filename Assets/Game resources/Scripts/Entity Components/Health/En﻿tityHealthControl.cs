using System;
using DG.Tweening;
using UnityEngine;

public class EntityHealthControl : MonoBehaviour
{
    [SerializeField] protected Animator _animator;
    [SerializeField] protected Transform _rotationParent;
    [SerializeField] protected IntVariable _maxHealth;
    [SerializeField] protected Transform _enemyAttackPoint;
    [SerializeField] private bool _needDestroyAfterDeath;
    
    public Action OnDeathStart;
    public Action OnDeathEnd;
    public Action OnHealthChanged;
    
    public event Action<EntityComponentsData, int> OnEnemyAttackUs;
    public bool IsDead => _isDead;
    public float HealthPercent => _currentHealth / _maxHealth.Value;
    public Transform RotationParent => _rotationParent;
    public Transform EnemyAttackPoint => _enemyAttackPoint;
    public Animator Animator => _animator;
    
    private const float DiveDuration = 1f;
    protected const float DiveDelay = 2.5f;
    private const float DiveDepth = 4f;
    
    protected bool _isDead;
    [SerializeField] protected float _currentHealth;

    protected virtual void Start() => _currentHealth = _maxHealth.Value;

    public void TakeDamage(EntityComponentsData attaker, int damage)
    {
        if (_isDead) return;
        
        _currentHealth -= damage;
        OnHealthChanged?.Invoke();
        OnEnemyAttackUs?.Invoke(attaker, damage);
        
        if (_currentHealth > 0 || _isDead) 
            return;
        
        _isDead = true;
        PlayDeathAnimation();
    }
    
    private void PlayDeathAnimation()
    {
        var sequence = DOTween.Sequence();
        
        sequence.AppendCallback(StartDeath);
        sequence.AppendInterval(DiveDelay);
        sequence.Append(transform.parent.DOLocalMoveY(transform.parent.localPosition.y - DiveDepth, DiveDuration));
        sequence.AppendCallback(() =>
        {
            OnDeathEnd?.Invoke();
            
            if (_needDestroyAfterDeath)
                Destroy(transform.parent.gameObject);
        }); 
    }

    protected virtual void StartDeath() => OnDeathStart?.Invoke();
}