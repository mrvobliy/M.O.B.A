using DG.Tweening;
using UnityEngine;

public class CreepHealthControl : EntityHealthControl
{
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
        if (_componentsData.RotationRoot == null || data.EntityType != EntityType.Hero) return;
		
        _componentsData.RotationRoot.DOLocalMove(-_componentsData.RotationRoot.forward * ReboundForce, ReboundTime).OnComplete(() 
            => _componentsData.RotationRoot.DOLocalMove(new Vector3(0, 0, 0), ReboundTime));
    }
    
    protected override void StartDeath()
    {
        base.StartDeath();
        _componentsData.NavMeshAgent.enabled = false;
        _componentsData.Animator.SetBool(AnimatorHash.IsDeath, true);
    }
}