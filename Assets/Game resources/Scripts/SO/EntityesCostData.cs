using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class EntityesCostData : ScriptableObject
{
    public List<EntityCost> EntityCosts;

    public int GetFinisherCost(EntityType entityType)
    {
        var cost = EntityCosts.FirstOrDefault(x => x.EntityType == entityType);
        return cost?.FinisherCost ?? 0;
    }
    
    public int GetHelpCost(EntityType entityType)
    {
        var cost = EntityCosts.FirstOrDefault(x => x.EntityType == entityType);
        return cost?.FinisherCost ?? 0;
    }
}