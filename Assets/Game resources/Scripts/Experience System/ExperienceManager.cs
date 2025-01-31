using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    [SerializeField] private EntityesStatsData _entitiesStatsData;
    
    private void OnEnable() => EventsBase.EntityDeath += TrySetHeroExperience;
    private void OnDisable() => EventsBase.EntityDeath -= TrySetHeroExperience;
    
    private void TrySetHeroExperience(EntityComponentsData deadHeroData, List<Attackers> attackers)
    {
         if (attackers.Count <= 0 || deadHeroData.EntityType == EntityType.Tower) return;
        
        var attackersSortByDamage = attackers.OrderBy(x => x.SummaryDamage).ToList();
        
        var experienceForFinisher = _entitiesStatsData.GetExperience(deadHeroData.EntityType);
        var experienceForHelper = 0f;
        
        
        //ПРОСЧЁТ БОНУСА ОПЫТА ЗА ГЕРОЯ
        if (deadHeroData.EntityType == EntityType.Hero) 
            experienceForFinisher += (int)(deadHeroData.HeroExperienceControl.Level * 50 * 2.14f);
        
        
        //ПРОСЧЁТ ОПЫТА ДЛЯ ПОМОГАЮЩЕГО
        experienceForHelper += experienceForFinisher * 0.87f;
        
        
        //НАЗНАЧЕНИЕ ОПЫТА ДЛЯ ДОБИВАЮЩЕГО
        attackersSortByDamage[0].ComponentsData.HeroGoldControl.SetGold(experienceForFinisher);

        
        //НАЗНАЧЕНИЕ ОПЫТА ДЛЯ ПОМОГАЮЩИХ ПО ВНЕСЁННОМУ УРОНУ
        if (attackers.Count <= 1) return;

        var summaryDamage = attackers.Sum(x => x.SummaryDamage);
        
        for (var i = 1; i < attackers.Count; i++)
        {
            var costForHelperByDamage = experienceForHelper * GetInflictedDamageCoefficient(summaryDamage, attackers[i].SummaryDamage);
            attackers[i].ComponentsData.HeroGoldControl.SetGold((int)costForHelperByDamage);
        }
    }
    
    private float GetInflictedDamageCoefficient(int summaryDamage, int heroDamage)
    {
        var damageFraction = 100 / (summaryDamage / heroDamage);

        if (damageFraction is > 0 and <= 40)
            return 0.3f;
        
        if (damageFraction is > 39 and < 80)
            return 0.5f;

        return 1;
    }
}