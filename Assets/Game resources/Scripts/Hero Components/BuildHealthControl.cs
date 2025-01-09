using UnityEngine;
using UnityEngine.AI;

public class BuildHealthControl : EntityHealthControl
{
    [SerializeField] private Collider _collider;
    [SerializeField] private NavMeshObstacle _navMeshObstacle;
    [SerializeField] private Healthbar _healthBarPrefab;
    [SerializeField] private Rigidbody[] _rigidBodies;
    [SerializeField] private ParticleSystem _explosionEffect;
    
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
        
        _explosionEffect.Play();

        foreach (var element in _rigidBodies)
        {
            element.isKinematic = false;

            var exploreDir = new Vector3
            {
                x = Random.Range(-1.0f, 1.0f),
                y = Random.Range(0f, 1.5f),
                z = Random.Range(-1.0f, 1.0f)
            };

            element.AddForce(exploreDir * 200);
        }
        
        Invoke(nameof(DisableComponents), DiveDelay);
    }

    private void DisableComponents()
    {
        foreach (var rigidbody in _rigidBodies) 
            rigidbody.isKinematic = true;
    }
}
