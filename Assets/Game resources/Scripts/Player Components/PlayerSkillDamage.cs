using UnityEngine;

public class PlayerSkillDamage : MonoBehaviour
{
    [SerializeField] private IntVariable _damageValue;
    [SerializeField] private float _timeToDestroy;

    private EntityComponentsData _ourEntityComponentsData;
    
    public void Init(EntityComponentsData heroData)
    {
        _ourEntityComponentsData = heroData;
        Invoke(nameof(InvokeDestroy), _timeToDestroy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player")) return;
        
        if (!other.TryGetComponent(out EntityComponentsData entityComponentsData)) return;
        
        if (entityComponentsData.EntityTeam == _ourEntityComponentsData.EntityTeam) return;
        
        if (entityComponentsData.EntityHealthControl.IsDead) return;
        
        entityComponentsData.EntityHealthControl.TakeDamage(_ourEntityComponentsData, _damageValue.Value);
        entityComponentsData.EntityMoveControl.TryStun(100, 3);
    }

    private void InvokeDestroy() => Destroy(gameObject);
}