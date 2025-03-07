using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class HeroStatsData : EntityStatsData
{
    private bool IsShowHeroStats => _type is EntityType.Hero;
    
    [Space]
    [BoxGroup("Hero stats", ShowLabel = false)]
    [SerializeField, ShowIf(nameof(IsShowHeroStats))] protected int _mana;
    
    [BoxGroup("Hero stats", ShowLabel = false)]
    [SerializeField, ShowIf(nameof(IsShowHeroStats))] protected int _healthRegeneration;
    
    [BoxGroup("Hero stats", ShowLabel = false)]
    [SerializeField, ShowIf(nameof(IsShowHeroStats))] protected int _manaRegeneration;
    
    [BoxGroup("Hero stats", ShowLabel = false)]
    [SerializeField, ShowIf(nameof(IsShowHeroStats))] protected float _skillsCdPercent;
    
    public int Mana => _mana;
    public int HealthRegeneration => _healthRegeneration;
    public int ManaRegeneration => _manaRegeneration;
    public float SkillsCdPercent => _skillsCdPercent;
}