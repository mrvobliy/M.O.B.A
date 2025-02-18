using UnityEngine;

public class CreepMoveControl : EntityMoveControl
{
    private const float OffsetStoppingDistance = 2;
    
    public Transform SpawnPont { get; private set; }
    
    private Transform _target;
    private bool _isNeedStoppingDistance;
    
    protected override Vector3 GetTarget()
    {
        if (_target == null)
        {
            transform.parent.rotation = SpawnPont.rotation;
            return transform.position;
        } 
        
        _componentsData.NavMeshAgent.stoppingDistance = !_isNeedStoppingDistance ? 0 : 
            _componentsData.EntityAttackControl.AttackDistance / OffsetStoppingDistance;
        return _target.position;
    }
    
    public void SetTarget(Transform target, bool isNeedStoppingDistance)
    {
        _target = target;
        _isNeedStoppingDistance = isNeedStoppingDistance;
    }

    public void SetSpawnPoint(Transform point) => SpawnPont = point;
}