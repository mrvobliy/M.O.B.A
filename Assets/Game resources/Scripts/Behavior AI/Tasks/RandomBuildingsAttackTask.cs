using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class RandomBuildingsAttackTask : Action
{
    [SerializeField] private HeroAIMoveControl _aiMoveControl;
    [SerializeField] private EntityComponentsData _entityComponentsData;

    private EntityComponentsData _target;
    
    public override void OnStart()
    {
        base.OnStart();
        UpdateTarget();
    }

    public override TaskStatus OnUpdate()
    {
        if (_target == null || _target.EntityHealthControl.IsDead)
            UpdateTarget();
        
        _aiMoveControl.SetAiTarget(_target.transform.parent);
        return TaskStatus.Running;
    }

    private void UpdateTarget() => _target = BuildingsManager.Instance.GetNearestRandomBuild(_entityComponentsData);
}