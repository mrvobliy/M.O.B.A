using UnityEngine;

public class FormulasBase : MonoBehaviour
{
    private const float ExperienceKillStreakValue = 3.25f;
    
    public static float GetInflictedDamageCoefficient(int summaryDamage, int heroDamage)
    {
        var damageFraction = 100 / (summaryDamage / heroDamage);

        if (damageFraction is > 0 and <= 40) return 0.3f;
        if (damageFraction is > 39 and < 80) return 0.5f;
        return 1;
    }
    
    public static double CalculateExperience(int killCount) => (1.25 * Mathf.Pow(ExperienceKillStreakValue, 2) - 2.5 * ExperienceKillStreakValue) * killCount;
}