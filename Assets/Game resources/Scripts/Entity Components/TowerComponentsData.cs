using Sirenix.OdinInspector;
using UnityEngine;

public class TowerComponentsData : MonoBehaviour
{
    [SerializeField] private EntityComponentsData _componentsData;

    public EntityComponentsData ComponentsData => _componentsData;

    [Button]
    private void SetData() => _componentsData = GetComponentInChildren<EntityComponentsData>();
}