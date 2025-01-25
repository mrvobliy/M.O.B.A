using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class GoToSpawnAndWaitTask : Action
{
    [SerializeField] private CreepMoveControl _creepMoveControl;

    public override TaskStatus OnUpdate()
    {
        base.OnUpdate();
        _creepMoveControl.SetTarget(_creepMoveControl.SpawnPont, false);
        return TaskStatus.Running;
    }
}