using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KillStreakManager : MonoBehaviour
{
    [SerializeField] private List<SeriesKillsInfo> _killStreakInfos;

    private float _valueExperienceKillStreak = 3.25f;

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
        
        //РАСЧЁТ И НАЗНАЧЕНИЕ БОНУСА ЗОЛОТА И ОПЫТА ЗА СЕРИЮ УБИЙСТВ ДЛЯ ДОБИВАЮЩЕГО
        if (finisherInfo != null)
        {
            finisherInfo.CoutKills++;
            
            //ЗОЛОТО ЗА СЕРИЮ
            var killStreakCost = finisherInfo.GetCountSeriesKills() * 15 * deadHeroData.HeroExperienceControl.Level;
            finisherInfo.HeroData.HeroGoldControl.SetGold(killStreakCost);
            
            //ОПЫТ ЗА СЕРИЮ
            var killStreakExperience = (1.25 * Mathf.Pow(_valueExperienceKillStreak, 2) - 2.5 * _valueExperienceKillStreak) * finisherInfo.GetCountSeriesKills();
            finisherInfo.HeroData.HeroExperienceControl.SetExperience((int)killStreakExperience);
        }
        else
        {
            var newInfo = new SeriesKillsInfo(0, finisher);
            _killStreakInfos.Add(newInfo);
        }
        
        //РАСЧЁТ БОНУСА ЗОЛОТА И ОПЫТА ЗА ПЕРЫВАНИЕ СЕРИИ УБИЙСТВ
        var deadInfo = _killStreakInfos.FirstOrDefault(info => info.HeroData == deadHeroData);
        var breakKillStreakCost = 0;
        var breakKillStreakExperience = 0d;
        
        if (deadInfo != null)
        {
            if (deadInfo.CoutKills > 0)
            {
                //ЗОЛОТО ЗА ПРЕРЫВАНИЕ СЕРИИ
                breakKillStreakCost = deadInfo.GetCountSeriesKills() * 15 * deadHeroData.HeroExperienceControl.Level;
                
                //ОПЫТ ЗА ПРЕРЫВАНИЕ СЕРИИ
                breakKillStreakExperience = (1.25 * Mathf.Pow(_valueExperienceKillStreak, 2) - 2.5 * _valueExperienceKillStreak) * deadInfo.GetCountSeriesKills();
            }
            
            _killStreakInfos.Remove(deadInfo);
        }
        
        //НАЗНАЧЕНИЕ БОНУСА ЗОЛОТА И ОПЫТА ЗА ПЕРЫВАНИЕ СЕРИИ УБИЙСТВ ДЛЯ ДОБИВАЮЩЕГО
        finisher.HeroGoldControl.SetGold(breakKillStreakCost);
        finisher.HeroExperienceControl.SetExperience((int)breakKillStreakExperience);
        
        
        //НАЗНАЧЕНИЕ БОНУСА ЗОЛОТА И ОПЫТА ЗА ПЕРЫВАНИЕ СЕРИИ УБИЙСТВ ДЛЯ ПОМОГАЮЩЕГО
        if (attackers.Count <= 1) return;

        var summaryDamage = attackers.Sum(x => x.SummaryDamage);
        
        for (var i = 1; i < attackers.Count; i++)
        {
            var inflictedDamageCoefficient = GetInflictedDamageCoefficient(summaryDamage, attackers[i].SummaryDamage);
            
            //ЗОЛОТО ЗА ПРЕРЫВАНИЕ СЕРИИ
            var breakKillStreakCostForHelper = breakKillStreakCost * inflictedDamageCoefficient;
            attackers[i].ComponentsData.HeroGoldControl.SetGold((int)breakKillStreakCostForHelper);
            
            //ОПЫТ ЗА ПРЕРЫВАНИЕ СЕРИИ
            var breakKillStreakExperienceForHelper = breakKillStreakExperience * inflictedDamageCoefficient;
            attackers[i].ComponentsData.HeroExperienceControl.SetExperience((int)breakKillStreakExperienceForHelper);
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

    public int GetCountSeriesKills() => CoutKills >= 5 ? 5 : CoutKills;
}