using UnityEngine;

public class LaneCreepMoveControl : EntityMoveControl
{
    private Transform[] _waypoints;
    private bool _pathIsFinished;
    private int _pathIndex;
    
    private EntityComponentsData ClosestEnemyInVisibility =>
        _entityComponentsData.EntityAttackControl.GetClosestEnemyInVisibilityArea();
    
    public void SetWaypoints(Transform[] waypoints) => _waypoints = waypoints;
    
    protected override Vector3 GetTarget()
    {
        _agent.stoppingDistance = 0f;

        if (ClosestEnemyInVisibility != null)
        {
            _agent.stoppingDistance = _entityComponentsData.EntityAttackControl.AttackDistance;
            return ClosestEnemyInVisibility.transform.position;
        }

        if (_pathIsFinished) 
            return transform.position;
		
        var next = _waypoints[_pathIndex];
        var distanceTo = (transform.position.SetY(0f) - next.position.SetY(0f)).magnitude;
        
        if (distanceTo >= 0.5f) 
            return next.position;
		
        _pathIndex++;
		
        if (_pathIndex == _waypoints.Length) 
            _pathIsFinished = true;

        return next.position;
    }
}