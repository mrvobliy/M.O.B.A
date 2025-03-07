using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class HeroStatsData : EntityStatsData
{
    [Space]
    [BoxGroup("Hero stats", ShowLabel = false)]
    [SerializeField] protected int _mana;
    
    [BoxGroup("Hero stats", ShowLabel = false)]
    [SerializeField] protected int _healthRegeneration;
    
    [BoxGroup("Hero stats", ShowLabel = false)]
    [SerializeField] protected int _manaRegeneration;
    
    [BoxGroup("Hero stats", ShowLabel = false)]
    [SerializeField] protected float _skillsCdPercent;
    
    public int Mana => _mana;
    public int HealthRegeneration => _healthRegeneration;
    public int ManaRegeneration => _manaRegeneration;
    public float SkillsCdPercent => _skillsCdPercent;
}