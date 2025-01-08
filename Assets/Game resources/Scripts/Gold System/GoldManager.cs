using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    [TableList]
    [SerializeField] private List<EntityCost> _entityCosts;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void SetHeroGold()
    {
        
    }
}

[Serializable]
public class EntityCost
{
    public EntityType EntityType;
    public int Cost;
}
