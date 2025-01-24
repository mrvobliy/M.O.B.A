using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class HeroFoundCondition : Conditional
{
    [SerializeField] private EntityAttackControl _entityAttackControl;
    
    public override TaskStatus OnUpdate()
    {
        var target = _entityAttackControl.GetClosestEnemyInVisibilityArea();

        if (target == null ||
            target.EntityType != EntityType.Hero) 
            return TaskStatus.Failure;
        
        return TaskStatus.Success;
    }
}
