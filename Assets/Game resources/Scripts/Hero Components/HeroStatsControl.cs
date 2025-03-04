using UnityEngine;

public class HeroStatsControl : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private int _armor;
    [SerializeField] private int _mana;
    [SerializeField] private int _damage;
    [SerializeField] private int _attackSpeed;
    [SerializeField] private int _moveSpeed;
    [SerializeField] private float _skillsCdPercent;
    
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
