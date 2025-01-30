using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class EntityesStatsData : ScriptableObject
{
    public List<EntityesCosts> CostsList;
    public List<EntityesExperinece> ExperineceList;

    public int GetFinisherCost(EntityType entityType)
    {
        var cost = CostsList.FirstOrDefault(x => x.EntityType == entityType);
        return cost?.FinisherCost ?? 0;
    }
    
    public int GetHelpCost(EntityType entityType)
    {
        var cost = CostsList.FirstOrDefault(x => x.EntityType == entityType);
        return cost?.HelpCost ?? 0;
    }
    
    public int GetExperience(EntityType entityType)
    {
        var experience = ExperineceList.FirstOrDefault(x => x.EntityType == entityType);
        return experience?.Experience ?? 0;
    }
}