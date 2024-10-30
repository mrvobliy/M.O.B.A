using UnityEngine;

public class PlayerSkillDamage : MonoBehaviour
{
    [SerializeField] private int _damageValue;
    [SerializeField] private float _timeToDestroy;

    private Target _target;
    
    public void Init(Target target)
    {
        _target = target;
        Invoke(nameof(InvokeDestroy), _timeToDestroy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player")) return;
        
        if (!other.TryGetComponent(out Target attackTarget)) return;
        
        if (attackTarget.Team == _target.Team) return;
        
        if (attackTarget.IsDead) return;
        
        attackTarget.TakeDamage(_target, _damageValue);
        attackTarget.TryStun(100, 3);
    }

    private void InvokeDestroy()
    {
        Destroy(gameObject);
    }
}
