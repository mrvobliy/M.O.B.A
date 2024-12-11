using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    [TableList]
    [SerializeField] private List<EntityCost> _entityCosts;
    
    public static GoldManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public int GetCostForEntity(EntityComponentsData componentsData)
    {
        switch (componentsData.EntityType)
        {
            
        }

        return 1;
    }
}

[Serializable]
public class EntityCost
{
    public EntityType EntityType;
    public int Cost;
}
