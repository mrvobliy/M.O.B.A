using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class HeroAttackTask : Action
{
    [SerializeField] private HeroMoveControl _moveControl;
    [SerializeField] private EntityAttackControl _entityAttackControl;
    [SerializeField] private EntityComponentsData _entityComponentsData;
    
    public override TaskStatus OnUpdate()
    {
        var target = _entityAttackControl.GetClosestHeroInVisibilityArea();
        _moveControl.SetAiTarget(target.transform.parent);
        
        return TaskStatus.Running;
    }
}