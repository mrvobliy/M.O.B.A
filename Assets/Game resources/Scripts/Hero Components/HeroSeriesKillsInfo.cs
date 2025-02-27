using UnityEngine;

public class HeroSeriesKillsInfo : MonoBehaviour
{
    public int CoutKills { get; private set; }
    public bool IsFirstBlood { get; private set; } = true;
    
    public void AddKills()
    {
        CoutKills++;

        if (IsFirstBlood && CoutKills == 2) 
            IsFirstBlood = false;

        if (CoutKills > 5)
            CoutKills = 5;
    }

    public void ResetSeries() => CoutKills = 0;
}
