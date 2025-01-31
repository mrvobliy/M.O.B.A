using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KillStreakManager : MonoBehaviour
{
    [SerializeField] private List<SeriesKillsInfo> _killStreakInfos;

    private void OnEnable()
    {
        EventsBase.EntityDeath += UpdateKillStreakInfo;
        _killStreakInfos = new List<SeriesKillsInfo>();
    }

    private void OnDisable() => EventsBase.EntityDeath -= UpdateKillStreakInfo;

    private void UpdateKillStreakInfo(EntityComponentsData deadHeroData, List<Attackers> attackers)
    {
        if (attackers.Count == 0 || deadHeroData.EntityType != EntityType.Hero) return;

        var finisher = attackers[0].ComponentsData;
        UpdateKillStreakFinisher(finisher, deadHeroData);
        ProcessKillStreakBreak(deadHeroData, finisher, attackers);
    }

    private void UpdateKillStreakFinisher(EntityComponentsData finisher, EntityComponentsData deadHeroData)
    {
        var finisherInfo = _killStreakInfos.FirstOrDefault(info => info.HeroData == finisher);

        if (finisherInfo == null)
        {
            _killStreakInfos.Add(new SeriesKillsInfo(0, finisher));
            return;
        }

        finisherInfo.CoutKills++;
        AssignKillStreakBonusesForFinisher(finisherInfo, deadHeroData);
    }

    private void AssignKillStreakBonusesForFinisher(SeriesKillsInfo finisherInfo, EntityComponentsData deadHeroData)
    {
        var killStreakCost = finisherInfo.GetCountSeriesKills() * 15 * deadHeroData.HeroExperienceControl.Level;
        var killStreakExperience = FormulasBase.CalculateExperience(finisherInfo.GetCountSeriesKills());
        
        finisherInfo.HeroData.HeroGoldControl.SetGold(killStreakCost);
        finisherInfo.HeroData.HeroExperienceControl.SetExperience((int)killStreakExperience);
    }

    private void ProcessKillStreakBreak(EntityComponentsData deadHeroData, EntityComponentsData finisher, List<Attackers> attackers)
    {
        var deadInfo = _killStreakInfos.FirstOrDefault(info => info.HeroData == deadHeroData);

        var breakKillStreakCost = 0;
        double breakKillStreakExperience = 0;

        if (deadInfo != null)
        {
            if (deadInfo.CoutKills > 0)
            {
                breakKillStreakCost = deadInfo.GetCountSeriesKills() * 15 * deadHeroData.HeroExperienceControl.Level;
                breakKillStreakExperience = FormulasBase.CalculateExperience(deadInfo.GetCountSeriesKills());
            }

            _killStreakInfos.Remove(deadInfo);
        }

        AssignBreakKillStreakBonusesForFinisher(finisher, breakKillStreakCost, breakKillStreakExperience);
        AssignBreakKillStreakBonusesForHelpers(attackers, breakKillStreakCost, breakKillStreakExperience / 15);
    }

    private void AssignBreakKillStreakBonusesForFinisher(EntityComponentsData finisher, int breakKillStreakCost, double breakKillStreakExperience)
    {
        finisher.HeroGoldControl.SetGold(breakKillStreakCost);
        finisher.HeroExperienceControl.SetExperience((int)breakKillStreakExperience);
    }

    private void AssignBreakKillStreakBonusesForHelpers(List<Attackers> attackers, int breakKillStreakCost, double breakKillStreakExperience)
    {
        if (attackers.Count <= 1) return;

        var summaryDamage = attackers.Sum(x => x.SummaryDamage);

        for (var i = 1; i < attackers.Count; i++)
        {
            var inflictedDamageCoefficient = FormulasBase.GetInflictedDamageCoefficient(summaryDamage, attackers[i].SummaryDamage);

            attackers[i].ComponentsData.HeroGoldControl.SetGold((int)(breakKillStreakCost * inflictedDamageCoefficient));
            attackers[i].ComponentsData.HeroExperienceControl.SetExperience((int)(breakKillStreakExperience * inflictedDamageCoefficient));
        }
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

    public int GetCountSeriesKills() => Mathf.Min(CoutKills, 5);
}