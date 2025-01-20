using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class NeedAmountHealthCondition : Conditional
{
    [SerializeField] private EntityHealthControl _entityHealthControl;
    [SerializeField] private float _needAmountHealth;
    
    public override TaskStatus OnUpdate()
    {
        return _entityHealthControl.HealthPercent * 100 < _needAmountHealth
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}