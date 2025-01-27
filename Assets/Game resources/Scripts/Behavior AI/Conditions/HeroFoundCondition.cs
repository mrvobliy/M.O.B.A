using System.Linq;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class HeroFoundCondition : Conditional
{
    [SerializeField] private EntityAttackControl _entityAttackControl;
    
    public override TaskStatus OnUpdate()
    {
        //var target = _entityAttackControl.GetClosestEnemyInVisibilityArea();

        var targets = _entityAttackControl.ClosestEnemyInVisibilityArea;
        return targets.Any(target => target.EntityType == EntityType.Hero) ? TaskStatus.Success : TaskStatus.Failure;
    }
}