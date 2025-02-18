using UnityEngine;

public class LaneCreepMoveControl : EntityMoveControl
{
    private const float OffsetStoppingDistance = 2;
    
    private Transform[] _waypoints;
    private bool _pathIsFinished;
    private int _pathIndex;
    
    private EntityComponentsData ClosestEnemyInVisibility =>
        _componentsData.EntityAttackControl.GetClosestEnemyInVisibilityArea();
    
    public void SetWaypoints(Transform[] waypoints) => _waypoints = waypoints;
    
    protected override Vector3 GetTarget()
    {
        _componentsData.NavMeshAgent.stoppingDistance = 0f;

        if (ClosestEnemyInVisibility != null)
        {
            _componentsData.NavMeshAgent.stoppingDistance = _componentsData.EntityAttackControl.AttackDistance / OffsetStoppingDistance;
            return ClosestEnemyInVisibility.transform.position;
        }

        if (_pathIsFinished) 
            return transform.parent.position;
		
        var next = _waypoints[_pathIndex];
        var distanceTo = (transform.parent.position.SetY(0f) - next.position.SetY(0f)).magnitude;
        
        if (distanceTo >= 0.5f) 
            return next.position;
		
        _pathIndex++;
		
        if (_pathIndex == _waypoints.Length) 
            _pathIsFinished = true;

        return next.position;
    }
}