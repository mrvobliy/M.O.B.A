using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    [SerializeField] private EntityesCostData _entitiesCostData;

    private bool _isFirstBlood = true;

    private void OnEnable() => EventsBase.EntityDeath += TrySetHeroGold;

    private void OnDisable() => EventsBase.EntityDeath -= TrySetHeroGold;

    private void TrySetHeroGold(EntityComponentsData componentsData, List<Attackers> attackers)
    {
        if (attackers.Count <= 0) return;
        
        var attackersSortByDamage = attackers.OrderBy(x => x.SummaryDamage).ToList();

        var costForFinisher = _entitiesCostData.GetFinisherCost(componentsData.EntityType);
        var costForHelper = _entitiesCostData.GetHelpCost(componentsData.EntityType);

        if (componentsData.EntityType == EntityType.Hero)
        {
            costForFinisher += componentsData.HeroExperienceControl.Level * 10;
            costForHelper += componentsData.HeroExperienceControl.Level * 10;

            if (_isFirstBlood)
            {
                costForFinisher += 10;
                costForHelper += 10;
                _isFirstBlood = false;
            }
        }
        
        attackersSortByDamage[0].ComponentsData.HeroGoldControl.SetGold(costForFinisher);

        for (var i = 1; i < attackers.Count; i++) 
            attackersSortByDamage[i].ComponentsData.HeroGoldControl.SetGold(costForHelper);
    }
}