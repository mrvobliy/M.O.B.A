using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class EntityStatsData
{
    [TableColumnWidth(150, Resizable = false)]
    [PreviewField(Alignment = ObjectFieldAlignment.Center)]
    [BoxGroup("Preview", ShowLabel = false)]
    [SerializeField] protected Sprite _view;
    
    [Space]
    [BoxGroup("Preview", ShowLabel = false)]
    [SerializeField] protected string _name;
    
    [BoxGroup("Preview", ShowLabel = false)]
    [SerializeField] protected EntityType _type;
    
    [Space]
    [BoxGroup("Base stats", ShowLabel = false)]
    [SerializeField] protected int _health;
    
    [BoxGroup("Base stats", ShowLabel = false)]
    [SerializeField] protected int _armor;
    
    [BoxGroup("Base stats", ShowLabel = false)]
    [SerializeField] protected int _damage;
    
    [BoxGroup("Base stats", ShowLabel = false)]
    [SerializeField] protected int _attackDistance;
    
    [BoxGroup("Base stats", ShowLabel = false)]
    [SerializeField] protected int _attackSpeed;
    
    [BoxGroup("Base stats", ShowLabel = false)]
    [SerializeField] protected int _moveSpeed;
    
    
    [Space]
    [BoxGroup("Costs, Experience", ShowLabel = false)]
    [SerializeField] protected int _finisherCost;
    
    [BoxGroup("Costs, Experience", ShowLabel = false)]
    [SerializeField] protected int _helpCost;
    
    [BoxGroup("Costs, Experience", ShowLabel = false)]
    [SerializeField] protected int _experience;

    public string Name => _name;
    public EntityType Type => _type;
    public int Health => _health;
    public int Armor => _armor;
    public int AttackSpeed => _attackSpeed;
    public int MoveSpeed => _moveSpeed;
    public int Damage => _damage;
    public int AttackDistance => _attackDistance;
    public int HelpCost => _helpCost;
    public int FinisherCost => _finisherCost;
    public int Experience => _experience;
}