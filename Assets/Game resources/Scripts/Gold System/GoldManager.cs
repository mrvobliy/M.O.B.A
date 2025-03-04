using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    [SerializeField] private EntityesStatsData _entitiesStatsData;

    private void OnEnable() => EventsBase.EntityDeath += TrySetHeroGold;
    private void OnDisable() => EventsBase.EntityDeath -= TrySetHeroGold;

    private void TrySetHeroGold(EntityComponentsData deadHeroData, List<Attackers> attackers)
    {
        if (attackers.Count <= 0) return;
        
        var attackersSortByDamage = attackers.OrderBy(x => x.SummaryDamage).ToList();

        var costForFinisher = _entitiesStatsData.GetFinisherCost(deadHeroData.EntityType);
        var costForHelper = _entitiesStatsData.GetHelpCost(deadHeroData.EntityType);

        //ПРОСЧЁТ БОНУСА ЗОЛОТА ОТДЕЛЬНО ЗА ГЕРОЯ
        if (deadHeroData.EntityType == EntityType.Hero)
        {
            costForFinisher += deadHeroData.HeroExperienceControl.Level * 10;
            costForHelper += deadHeroData.HeroExperienceControl.Level * 10;

            if (attackersSortByDamage[0].ComponentsData.HeroStatsControl.IsFirstBlood)
            {
                costForFinisher += 10;
                costForHelper += 10;
            }
        }

        //ПРОСЧЁТ ЗОЛОТА ОТДЕЛЬНО ЗА БАШНЮ
        if (deadHeroData.EntityType == EntityType.Tower)
        {
            costForFinisher = _entitiesStatsData.GetTowerFinisherCost(deadHeroData.TowerTier);
            costForHelper = _entitiesStatsData.GetTowerHelpCost(deadHeroData.TowerTier);
            
            var costForEveryone = _entitiesStatsData.GetTowerEveryoneCost(deadHeroData.TowerTier);
            var team = deadHeroData.EntityTeam == Team.Light ? Team.Dark : Team.Light;
            EventsBase.OnGetGoldForTeam(costForEveryone, team);
            costForFinisher -= costForEveryone;
        }
        
        //НАЗНАЧЕНИЕ НАГРАДЫ ДЛЯ ДОБИВАЮЩЕГО
        attackersSortByDamage[0].ComponentsData.HeroGoldControl.SetGold(costForFinisher);

        if (!attackersSortByDamage[0].ComponentsData.IsAi)
            GoldEffectManager.Instance.SpawnEffect(deadHeroData.transform, costForFinisher);
        
        //НАЗНАЧЕНИЕ НАГРАДЫ ДЛЯ ПОМОГАЮЩИХ ПО ВНЕСЁННОМУ УРОНУ
        if (attackers.Count <= 1) return;

        var summaryDamage = attackers.Sum(x => x.SummaryDamage);
        
        for (var i = 1; i < attackers.Count; i++)
        {
            var costForHelperByDamage = costForHelper * FormulasBase.GetInflictedDamageCoefficient(summaryDamage, attackers[i].SummaryDamage);
            attackers[i].ComponentsData.HeroGoldControl.SetGold((int)costForHelperByDamage);
        }
    }
}