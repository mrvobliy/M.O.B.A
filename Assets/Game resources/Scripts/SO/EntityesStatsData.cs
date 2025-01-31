using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class EntityesStatsData : ScriptableObject
{
    public List<EntityesCosts> CostsList;
    public List<EntityesExperinece> ExperineceList;
    public List<TowersTiersCosts> TowersTiersCostsList;
    public List<EntityesGameTimeExperinece> EntityesGameTimeExperienceList;

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
    
    public int GetTowerFinisherCost(TowerTier towerTier)
    {
        var cost = TowersTiersCostsList.FirstOrDefault(x => x.TowerTier == towerTier);
        return cost?.FinisherCost ?? 0;
    }
    
    public int GetTowerHelpCost(TowerTier towerTier)
    {
        var cost = TowersTiersCostsList.FirstOrDefault(x => x.TowerTier == towerTier);
        return cost?.HelpCost ?? 0;
    }
    
    public int GetTowerEveryoneCost(TowerTier towerTier)
    {
        var cost = TowersTiersCostsList.FirstOrDefault(x => x.TowerTier == towerTier);
        return cost?.EveryoneCost ?? 0;
    }
    
    public int GetGameTimeExperience(EntityType entityType)
    {
        var cost = EntityesGameTimeExperienceList.FirstOrDefault(x => x.EntityType == entityType);
        return cost?.Experience ?? 0;
    }
}