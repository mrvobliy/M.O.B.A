using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    [SerializeField] private EntityesStatsData _entitiesStatsData;

    private bool _isFirstBlood = true;

    private void OnEnable() => EventsBase.EntityDeath += TrySetHeroGold;
    private void OnDisable() => EventsBase.EntityDeath -= TrySetHeroGold;

    private void TrySetHeroGold(EntityComponentsData deadHeroData, List<Attackers> attackers)
    {
        if (attackers.Count <= 0) return;
        
        var attackersSortByDamage = attackers.OrderBy(x => x.SummaryDamage).ToList();

        var costForFinisher = _entitiesStatsData.GetFinisherCost(deadHeroData.EntityType);
        var costForHelper = _entitiesStatsData.GetHelpCost(deadHeroData.EntityType);

        if (deadHeroData.EntityType == EntityType.Hero)
        {
            costForFinisher += deadHeroData.HeroExperienceControl.Level * 10;
            costForHelper += deadHeroData.HeroExperienceControl.Level * 10;

            if (_isFirstBlood)
            {
                costForFinisher += 10;
                costForHelper += 10;
                _isFirstBlood = false;
            }
        }

        if (deadHeroData.EntityType == EntityType.Tower)
        {
            costForFinisher = _entitiesStatsData.GetTowerFinisherCost(deadHeroData.TowerTier);
            costForHelper = _entitiesStatsData.GetTowerHelpCost(deadHeroData.TowerTier);
            
            var costForEveryone = _entitiesStatsData.GetTowerEveryoneCost(deadHeroData.TowerTier);
            var team = deadHeroData.EntityTeam == Team.Light ? Team.Dark : Team.Light;
            EventsBase.OnGetGoldForTeam(costForEveryone, team);
            costForFinisher -= costForEveryone;
        }
        
        attackersSortByDamage[0].ComponentsData.HeroGoldControl.SetGold(costForFinisher);

        for (var i = 1; i < attackers.Count; i++) 
            attackersSortByDamage[i].ComponentsData.HeroGoldControl.SetGold(costForHelper);
    }
}