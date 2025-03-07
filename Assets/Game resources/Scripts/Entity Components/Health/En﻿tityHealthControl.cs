using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public abstract class EntityHealthControl : MonoBehaviour
{
    [SerializeField] protected EntityComponentsData _componentsData;
    [SerializeField] private bool _needDestroyAfterDeath;
    
    protected int _currentHealth;
    protected virtual int _healthBase { get; private set; } 
    protected virtual int _armorBase { get; private set; }
    
    public Action OnHealthChanged;
    public event Action<EntityComponentsData, int> OnEnemyAttackUs;
    
    public bool IsDead => _isDead;
    public float HealthPercent => _currentHealth / _healthBase;
    
    private const float DiveDuration = 1f;
    protected const float DiveDelay = 2.5f;
    private const float DiveDepth = 4f;
    
    protected bool _isDead;

    private List<Attackers> _attackers;

    protected virtual void Start()
    {
        _currentHealth = _healthBase;
        _attackers = new List<Attackers>();
    }

    public void TakeHeal(int value)
    {
        if (_isDead) return;
        
        _currentHealth += value;
        OnHealthChanged?.Invoke();
    }
    
    public void SetHealth(IntVariable health)
    {
        _healthBase = health.Value;
        _currentHealth = _healthBase;
        OnHealthChanged?.Invoke();
    }

    public void TakeDamage(EntityComponentsData attacker, int damage)
    {
        if (_isDead) return;
        
        _currentHealth -= 100 / (100 + _armorBase) * damage;
        OnHealthChanged?.Invoke();
        OnEnemyAttackUs?.Invoke(attacker, damage);
        UpdateAttackersData(attacker, damage);
        
        if (_currentHealth > 0 || _isDead) 
            return;
        
        EventsBase.OnEntityDeath(_componentsData, _attackers);
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
            _attackers.Remove(attacker);
            _attackers.Insert(0, attacker);
        }
    }
    
    private void PlayDeathAnimation()
    {
        var sequence = DOTween.Sequence();
        
        sequence.AppendCallback(StartDeath);
        sequence.AppendInterval(DiveDelay);
        sequence.Append(_componentsData.RotationRoot.DOLocalMoveY(_componentsData.RotationRoot.localPosition.y - DiveDepth, DiveDuration));
        sequence.AppendCallback(() =>
        {
            _componentsData.OnDeathEnd?.Invoke();
            
            if (_needDestroyAfterDeath) 
                Destroy(transform.parent.gameObject);
        }); 
    }

    protected virtual void StartDeath()
    {
        _componentsData.OnDeathStart?.Invoke();
        _componentsData.SetComponentsWorkState(false);
        
        if (_needDestroyAfterDeath) 
            _componentsData.Collider.enabled = false;
    }
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