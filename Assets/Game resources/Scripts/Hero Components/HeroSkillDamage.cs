using UnityEngine;

public class HeroSkillDamage : MonoBehaviour
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
        //if (other.transform.CompareTag("Player")) return;
        
        var entityComponentsData = other.GetComponentInChildren<EntityComponentsData>();
            
        if (entityComponentsData == null) return;
        
        if (entityComponentsData.EntityTeam == _ourEntityComponentsData.EntityTeam) return;
        
        switch (entityComponentsData.EntityType)
        {
            case EntityType.Tower:
            case EntityType.Throne:
                return;
        }

        if (entityComponentsData.EntityHealthControl.IsDead) return;
        
        entityComponentsData.EntityHealthControl.TakeDamage(_ourEntityComponentsData, _damageValue.Value);
        entityComponentsData.EntityMoveControl.TryStun(100, 3);
    }

    private void InvokeDestroy() => Destroy(gameObject);
}