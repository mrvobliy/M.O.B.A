using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class NeedAmountHealthCondition : Conditional
{
    [SerializeField] private EntityHealthControl _entityHealthControl;
    [SerializeField] private float _necessaryValueHealth;
    [Header("Is True When Current Value Below Necessary")]
    [SerializeField] private bool _isTrue;
    
    public override TaskStatus OnUpdate()
    {
        if (_isTrue)
        {
            return _entityHealthControl.HealthPercent * 100 < _necessaryValueHealth
                ? TaskStatus.Success
                : TaskStatus.Failure;
        }

        return _entityHealthControl.HealthPercent * 100 >= _necessaryValueHealth
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}