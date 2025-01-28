using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class EntityHealthControl : MonoBehaviour
{
    [SerializeField] protected Animator _animator;
    [SerializeField] protected Transform _rotationParent;
    [SerializeField] protected IntVariable _maxHealth;
    [SerializeField] protected Transform _enemyAttackPoint;
    [SerializeField] private bool _needDestroyAfterDeath;
    [SerializeField] protected float _currentHealth;
    
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

    private List<Attackers> _attackers;

    protected virtual void Start()
    {
        _currentHealth = _maxHealth.Value;
        _attackers = new List<Attackers>();
    }

    public void TakeHeal(int value)
    {
        if (_isDead) return;
        
        _currentHealth += value;
        OnHealthChanged?.Invoke();
    }

    public void TakeDamage(EntityComponentsData attacker, int damage)
    {
        if (_isDead) return;
        
        _currentHealth -= damage;
        OnHealthChanged?.Invoke();
        OnEnemyAttackUs?.Invoke(attacker, damage);
        UpdateAttackersData(attacker, damage);
        
        if (_currentHealth > 0 || _isDead) 
            return;
        
        _isDead = true;
        PlayDeathAnimation();
    }

    private void UpdateAttackersData(EntityComponentsData attackerData, int damage)
    {
        if (attackerData.EntityType != EntityType.Hero) 
            return;
        
        var attacker = _attackers.FirstOrDefault(attacker => attacker.ComponentsData == attackerData);

        if (attacker == null)
        {
            var newAttacker = new Attackers(attackerData, damage);
            _attackers.Add(newAttacker);
        }
        else
        {
            attacker.SummaryDamage += damage;
        }
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

public class Attackers
{
    public EntityComponentsData ComponentsData;
    public int SummaryDamage;

    public Attackers(EntityComponentsData componentsData, int summaryDamage)
    {
        ComponentsData = componentsData;
        SummaryDamage = summaryDamage;
    }
}