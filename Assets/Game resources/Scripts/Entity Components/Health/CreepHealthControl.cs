using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class CreepHealthControl : EntityHealthControl
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] protected Healthbar _healthBarPrefab;
    
    private const float ReboundTime = 0.12f;
    private const float ReboundForce = 0.4f;

    private void OnEnable()
    {
        OnEnemyAttackUs += RootRebound;
        var healthBar = Instantiate(_healthBarPrefab, UserInterface.Instance.transform);
        healthBar.Init(this);
    }

    private void OnDisable() => OnEnemyAttackUs -= RootRebound;

    private void RootRebound(EntityComponentsData data, int damage)
    {
        if (_rotationParent == null || data.EntityType != EntityType.Hero) return;
		
        _rotationParent.DOLocalMove(-_rotationParent.forward * ReboundForce, ReboundTime).OnComplete(() =>
        {
            _rotationParent.DOLocalMove(new Vector3(0, 0, 0), ReboundTime);
        });
    }
    
    protected override void StartDeath()
    {
        base.StartDeath();
        _agent.enabled = false;
        _animator.SetBool(AnimatorHash.IsDeath, true);
    }
}