using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class CreepsFoundCondition : Conditional
{
    [SerializeField] private EntityAttackControl _entityAttackControl;
    
    public override TaskStatus OnUpdate()
    {
        var target = _entityAttackControl.GetClosestEnemyInVisibilityArea();

        if (target == null ||
            target.EntityType == EntityType.Hero ||
            target.EntityType == EntityType.Tower ||
            target.EntityType == EntityType.Throne) 
            return TaskStatus.Failure;
        
        return TaskStatus.Success;
    }
}