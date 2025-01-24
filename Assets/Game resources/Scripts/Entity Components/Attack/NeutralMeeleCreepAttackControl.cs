using UnityEngine;

public class NeutralMeeleCreepAttackControl : MeeleCreepAttackControl
{
    [SerializeField] private NeutralCreepMoveControl _neutralCreepMoveControl;

    protected override void TryStartAttack()
    {
        if (!_neutralCreepMoveControl.IsAggressive && ClosestEnemyInAttackArea.Count <= 0) return;
        
        if (_insideAttack) return;

        _animator.SetTrigger(AnimatorHash.Attack);
        _insideAttack = true;
    }
}