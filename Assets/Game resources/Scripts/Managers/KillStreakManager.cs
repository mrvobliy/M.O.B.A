using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KillStreakManager : MonoBehaviour
{
    private void OnEnable() => EventsBase.EntityDeath += UpdateKillStreakInfo;
    private void OnDisable() => EventsBase.EntityDeath -= UpdateKillStreakInfo;

    private void UpdateKillStreakInfo(EntityComponentsData deadHeroData, List<Attackers> attackers)
    {
        if (attackers.Count == 0 || deadHeroData.EntityType != EntityType.Hero) return;

        var finisher = attackers[0].ComponentsData;
        AssignBonuses_KillStreak_ForFinisher(finisher, deadHeroData);
        ProcessKillStreakBreak(deadHeroData, finisher, attackers);
        EventsBase.OnHeroKillHero(finisher, deadHeroData);
    }

    private void AssignBonuses_KillStreak_ForFinisher(EntityComponentsData finisher, EntityComponentsData deadHeroData)
    {
        finisher.HeroSeriesKillsInfo.AddKills();
        
        var killStreakCost = finisher.HeroSeriesKillsInfo.CoutKills * 15 * deadHeroData.HeroExperienceControl.Level;
        var killStreakExperience = FormulasBase.CalculateExperience(finisher.HeroSeriesKillsInfo.CoutKills);
        
        finisher.HeroGoldControl.SetGold(killStreakCost);
        finisher.HeroExperienceControl.SetExperience((int)killStreakExperience);
    }

    private void AssignBonuses_BreakKillStreak_ForFinisher(EntityComponentsData finisher, int breakKillStreakCost, double breakKillStreakExperience)
    {
        finisher.HeroGoldControl.SetGold(breakKillStreakCost);
        finisher.HeroExperienceControl.SetExperience((int)breakKillStreakExperience);
    }

    private void AssignBonuses_BreakKillStreak_ForHelpers(List<Attackers> attackers, int breakKillStreakCost, double breakKillStreakExperience)
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
    
    private void ProcessKillStreakBreak(EntityComponentsData deadHeroData, EntityComponentsData finisher, List<Attackers> attackers)
    {
        var breakKillStreakCost = 0;
        double breakKillStreakExperience = 0;

        if (deadHeroData.HeroSeriesKillsInfo.CoutKills >= 0)
        {
            breakKillStreakCost = deadHeroData.HeroSeriesKillsInfo.CoutKills * 15 * deadHeroData.HeroExperienceControl.Level;
            breakKillStreakExperience = FormulasBase.CalculateExperience(deadHeroData.HeroSeriesKillsInfo.CoutKills);
            deadHeroData.HeroSeriesKillsInfo.ResetSeries();
        }

        AssignBonuses_BreakKillStreak_ForFinisher(finisher, breakKillStreakCost, breakKillStreakExperience);
        AssignBonuses_BreakKillStreak_ForHelpers(attackers, breakKillStreakCost, breakKillStreakExperience / 15);
    }
}