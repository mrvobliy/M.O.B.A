using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ConsistentlyBuildingsAttackTask : Action
{
    [SerializeField] private HeroMoveControl _moveControl;
    [SerializeField] private EntityComponentsData _entityComponentsData;
    
    public override TaskStatus OnUpdate()
    {
        var target = BuildingsManager.Instance.GetNearestNotBusyBuild(_entityComponentsData).transform;
        _moveControl.SetAiTarget(target.parent);
        
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        base.OnEnd();
        BuildingsManager.Instance.SetLeftBusyBuilding(_entityComponentsData, false);
    }
} 