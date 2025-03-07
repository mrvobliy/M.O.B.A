
public class HeroStatsControl : EntityStatsControl
{
    public int Mana;
    public int HealthRegeneration;
    public int ManaRegeneration;
    public float SkillsCdPercent;
    
    public int CoutKills { get; private set; }
    public bool IsFirstBlood { get; private set; } = true;
    
    private HeroStatsData _heroData;
    
    public void AddKills()
    {
        CoutKills++;

        if (IsFirstBlood && CoutKills == 2) 
            IsFirstBlood = false;

        if (CoutKills > 5)
            CoutKills = 5;
    }

    public void ResetSeries() => CoutKills = 0;

    protected override void SetStats()
    {
        Health = _heroData.Health;
        Armor = _heroData.Armor;
        AttackSpeed = _heroData.AttackSpeed;
        MoveSpeed = _heroData.MoveSpeed;
        Damage = _heroData.Damage;
        AttackDistance = _heroData.AttackDistance;
        Mana = _heroData.Mana;
        HealthRegeneration = _heroData.HealthRegeneration;
        ManaRegeneration = _heroData.ManaRegeneration;
        SkillsCdPercent = _heroData.SkillsCdPercent;
    }
}