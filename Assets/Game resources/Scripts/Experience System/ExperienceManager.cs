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
        
        
        //ПРИРОСТ В МИНУТУ
        if (deadHeroData.EntityType != EntityType.Hero)
        {
            var experienceGrowthPerMinute = GameTimeManager.Instance.Minutes * _entitiesStatsData.GetGameTimeExperience(deadHeroData.EntityType);
            experienceForFinisher += experienceGrowthPerMinute;
            experienceForHelper += experienceGrowthPerMinute;
        }
        
        //НАЗНАЧЕНИЕ ОПЫТА ДЛЯ ДОБИВАЮЩЕГО
        attackersSortByDamage[0].ComponentsData.HeroExperienceControl.SetExperience(experienceForFinisher);

        
        //НАЗНАЧЕНИЕ ОПЫТА ДЛЯ ПОМОГАЮЩИХ ПО ВНЕСЁННОМУ УРОНУ
        if (attackers.Count <= 1) return;

        var summaryDamage = attackers.Sum(x => x.SummaryDamage);
        
        for (var i = 1; i < attackers.Count; i++)
        {
            var costForHelperByDamage = experienceForHelper * FormulasBase.GetInflictedDamageCoefficient(summaryDamage, attackers[i].SummaryDamage);
            attackers[i].ComponentsData.HeroExperienceControl.SetExperience((int)costForHelperByDamage);
        }
    }
}