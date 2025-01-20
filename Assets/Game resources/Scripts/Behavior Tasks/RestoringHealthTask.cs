using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class RestoringHealthTask : Action
{
    [SerializeField] private HeroMoveControl _moveControl;
    [SerializeField] private EntityComponentsData _entityComponentsData;
    
    public override TaskStatus OnUpdate()
    {
        var target = HeroSpawnControl.Instance.GetPoint(_entityComponentsData.EntityTeam);
        _moveControl.SetAiTarget(target);
        
        return TaskStatus.Running;
    }
}