using UnityEngine;
using UnityEngine.AI;

public class BuildHealthControl : EntityHealthControl
{
    [SerializeField] protected Collider _collider;
    [SerializeField] protected NavMeshObstacle _navMeshObstacle;
    [SerializeField] protected Healthbar _healthBarPrefab;
    [SerializeField] protected Rigidbody[] _rigidBodies;
    [SerializeField] protected ParticleSystem _explosionEffect;
    
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

        PlayDestroyAnimation();
    }

    protected virtual void PlayDestroyAnimation() {}

    protected void DisableComponents()
    {
        foreach (var rigidbody in _rigidBodies) 
            rigidbody.isKinematic = true;
    }
}
