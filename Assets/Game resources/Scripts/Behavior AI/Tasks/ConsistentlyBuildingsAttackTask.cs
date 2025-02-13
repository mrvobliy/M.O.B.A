using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ConsistentlyBuildingsAttackTask : Action
{
    [SerializeField] private HeroAIMoveControl _aiMoveControl;
    [SerializeField] private EntityComponentsData _entityComponentsData;
    
    public override TaskStatus OnUpdate()
    {
        var target = BuildingsManager.Instance.GetNearestNotBusyBuild(_entityComponentsData).transform;
        _aiMoveControl.SetAiTarget(target.parent);
        
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        base.OnEnd();
        BuildingsManager.Instance.SetLeftBusyBuilding(_entityComponentsData, false);
    }
} 