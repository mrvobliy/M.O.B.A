using UnityEngine;
using UnityEngine.AI;

public class BuildHealthControl : EntityHealthControl
{
    [SerializeField] private Collider _collider;
    [SerializeField] private NavMeshObstacle _navMeshObstacle;
    [SerializeField] private Healthbar _healthBarPrefab;
    [SerializeField] private Rigidbody[] _rigidBodies;
    
    protected override void Start()
    {
        base.Start();
        var healthBar = Instantiate(_healthBarPrefab, UserInterface.Instance.transform);
        healthBar.Init(this);
    }

    protected override void StartDeath()
    {
        base.StartDeath();
        _navMeshObstacle.enabled = false;
        _animator.enabled = false;
        _collider.enabled = false;
        
        foreach (var rigidbody in _rigidBodies) 
            rigidbody.isKinematic = false;
    }
}
