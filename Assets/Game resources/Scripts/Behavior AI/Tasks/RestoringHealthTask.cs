using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class RestoringHealthTask : Action
{
    [SerializeField] private HeroMoveControl _moveControl;
    [SerializeField] private EntityComponentsData _entityComponentsData;
    [SerializeField] private SharedBool _isNeedHeal;

    private bool _isRunning;
    
    public override void OnStart()
    {
        base.OnStart();
        _isNeedHeal.Value = true;
    }

    public override TaskStatus OnUpdate()
    {
        _isRunning = true;
        
        var target = HeroSpawnManger.Instance.GetPoint(_entityComponentsData.EntityTeam);
        _moveControl.SetAiTarget(target);
        
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        base.OnEnd();
        
        _isNeedHeal.Value = false;
    }
}