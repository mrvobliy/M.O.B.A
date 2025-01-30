using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroExperienceControl : MonoBehaviour
{
    public int Level { get; private set; }
    
    private bool _isFirstBlood = true;

    private void OnEnable() => EventsBase.EntityDeath += TrySetHeroExperinece;
    private void OnDisable() => EventsBase.EntityDeath -= TrySetHeroExperinece;
    
    private void TrySetHeroExperinece(EntityComponentsData deadHeroData, List<Attackers> attackers)
    {
        // if (attackers.Count <= 0 || deadHeroData.EntityType == EntityType.Tower) return;
        //
        // var attackersSortByDamage = attackers.OrderBy(x => x.SummaryDamage).ToList();
        //
        // var costForFinisher = _entitiesCostExperienceData.GetFinisherCost(deadHeroData.EntityType);
        // var costForHelper = _entitiesCostExperienceData.GetHelpCost(deadHeroData.EntityType);
        //
        // if (deadHeroData.EntityType == EntityType.Hero)
        // {
        //     costForFinisher += deadHeroData.HeroExperienceControl.Level * 10;
        //     costForHelper += deadHeroData.HeroExperienceControl.Level * 10;
        //
        //     if (_isFirstBlood)
        //     {
        //         costForFinisher += 10;
        //         costForHelper += 10;
        //         _isFirstBlood = false;
        //     }
        // }
        //
        // attackersSortByDamage[0].ComponentsData.HeroGoldControl.SetGold(costForFinisher);
        //
        // for (var i = 1; i < attackers.Count; i++) 
        //     attackersSortByDamage[i].ComponentsData.HeroGoldControl.SetGold(costForHelper);
    }
}
