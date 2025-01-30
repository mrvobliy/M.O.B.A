using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KillStreakManager : MonoBehaviour
{
    [SerializeField] private List<SeriesKillsInfo> _killStreakInfos;

    private void OnEnable()
    {
        EventsBase.EntityDeath += UpdateSeriesKillsInfo;
        _killStreakInfos = new List<SeriesKillsInfo>();
    }

    private void OnDisable() => EventsBase.EntityDeath -= UpdateSeriesKillsInfo;
    
    private void UpdateSeriesKillsInfo(EntityComponentsData deadHeroData, List<Attackers> attackers)
    {
        if (attackers.Count <= 0 || deadHeroData.EntityType != EntityType.Hero) return;

        var finisher = attackers[0].ComponentsData;
        var finisherInfo = _killStreakInfos.FirstOrDefault(info => info.HeroData == finisher);
        
        //РАСЧЁТ И НАЗНАЧЕНИЕ БОНУСА ЗА СЕРИЮ УБИЙСТВ ДЛЯ ДОБИВАЮЩЕГО
        if (finisherInfo != null)
        {
            finisherInfo.CoutKills++;
            
            var killStreakCost = Mathf.Clamp(finisherInfo.CoutKills, 0, 5) * 15 * deadHeroData.HeroExperienceControl.Level;
            finisherInfo.HeroData.HeroGoldControl.SetGold(killStreakCost);
        }
        else
        {
            var newInfo = new SeriesKillsInfo(0, finisher);
            _killStreakInfos.Add(newInfo);
        }
        
        //РАСЧЁТ БОНУСА ЗА ПЕРЫВАНИЕ СЕРИИ УБИЙСТВ
        var deadInfo = _killStreakInfos.FirstOrDefault(info => info.HeroData == deadHeroData);
        var interruptionKillStreakCost = 0;
        
        if (deadInfo != null)
        {
            if (deadInfo.CoutKills > 0)
                interruptionKillStreakCost = Mathf.Clamp(deadInfo.CoutKills, 0, 5) * 15 * deadHeroData.HeroExperienceControl.Level;
            
            _killStreakInfos.Remove(deadInfo);
        }
        
        //НАЗНАЧЕНИЕ БОНУСА ЗА ПЕРЫВАНИЕ СЕРИИ УБИЙСТВ ДЛЯ ДОБИВАЮЩЕГО
        finisher.HeroGoldControl.SetGold(interruptionKillStreakCost);
        
        if (attackers.Count <= 1) return;

        var summaryDamage = attackers.Sum(x => x.SummaryDamage);
        
        //НАЗНАЧЕНИЕ БОНУСА ЗА ПЕРЫВАНИЕ СЕРИИ УБИЙСТВ ДЛЯ ПОМОГАЮЩЕГО
        for (var i = 1; i < attackers.Count; i++)
        {
            var interruptionKillStreakCostForHelper = interruptionKillStreakCost * GetInflictedDamageCoefficient(summaryDamage, attackers[i].SummaryDamage);
            attackers[i].ComponentsData.HeroGoldControl.SetGold((int)interruptionKillStreakCostForHelper);
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

public class SeriesKillsInfo
{
    public int CoutKills;
    public EntityComponentsData HeroData;

    public SeriesKillsInfo(int coutKills, EntityComponentsData heroData)
    {
        CoutKills = coutKills;
        HeroData = heroData;
    }
}