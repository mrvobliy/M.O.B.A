using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class NeutralAttackEnemy : Action
{
    [SerializeField] private NeutralCanAttackCondition _neutralCanAttackCondition;
    [SerializeField] private AICreepAttackControl _aiCreepAttackControl;
    [SerializeField] private CreepMoveControl _creepMoveControl;

    public override TaskStatus OnUpdate()
    {
        base.OnUpdate();

        var attacker = _neutralCanAttackCondition.AttackerData;
        var enemies = _aiCreepAttackControl.ClosestEnemyInAttackArea;

        if (enemies.Contains(attacker))
        {
            _aiCreepAttackControl.TryAttack(attacker, true); 
            _neutralCanAttackCondition.ResetAggressiveStateTime();
        } 
        
        _creepMoveControl.SetTarget(attacker.transform, true);
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        base.OnEnd();
        _neutralCanAttackCondition.ResetState();
    }
}