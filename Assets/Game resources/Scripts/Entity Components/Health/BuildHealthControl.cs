using UnityEngine;

public class BuildHealthControl : EntityHealthControl
{
    [SerializeField] protected Healthbar _healthBarPrefab;
    [SerializeField] protected Rigidbody[] _rigidBodies;
    [SerializeField] protected ParticleSystem _explosionEffect;
    
    protected override void Start()
    {
        base.Start();
        var healthBar = Instantiate(_healthBarPrefab, UserInterface.Instance.transform);
        healthBar.Init(_componentsData);
    }

    protected override void StartDeath()
    {
        base.StartDeath();
        _componentsData.NavMeshObstacle.enabled = false;
        _componentsData.Animator.enabled = false;
        _componentsData.Collider.enabled = false;

        PlayDestroyAnimation();
    }

    protected virtual void PlayDestroyAnimation() {}

    protected void DisableComponents()
    {
        foreach (var rigidbody in _rigidBodies) 
            rigidbody.isKinematic = true;
    }
}