using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu]
public class EntitiesStatsDataBase : ScriptableObject
{
    [TableList(ShowIndexLabels = true)]
    [Title("CREEPS, BUILDINGS STATS DATA", titleAlignment: TitleAlignments.Centered, horizontalLine: true, bold: true)]
    [SerializeField] protected List<EntityStatsData> _entitiesStatsData;
    
    [Space]
    [TableList(ShowIndexLabels = true)]
    [Title("HERO STATS DATA", titleAlignment: TitleAlignments.Centered, horizontalLine: true, bold: true)]
    [SerializeField] protected List<HeroStatsData> _heroesStatsData;
    
    public EntityStatsData GetEntityStatsData(string name) => 
        _entitiesStatsData.FirstOrDefault(x => x.Name == name);
    
    public EntityStatsData GetHeroStatsData(string name) => 
        _heroesStatsData.FirstOrDefault(x => x.Name == name);
}