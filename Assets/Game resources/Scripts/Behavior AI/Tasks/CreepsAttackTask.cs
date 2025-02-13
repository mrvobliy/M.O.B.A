using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class CreepsAttackTask : Action
{
    [SerializeField] private HeroAIMoveControl _aiMoveControl;
    [SerializeField] private EntityAttackControl _entityAttackControl;
    [SerializeField] private EntityComponentsData _entityComponentsData;
    
    public override TaskStatus OnUpdate()
    {
        var target = _entityAttackControl.GetClosestEnemyInVisibilityArea();
        _aiMoveControl.SetAiTarget(target.transform.parent);
        
        return TaskStatus.Running;
    }
}