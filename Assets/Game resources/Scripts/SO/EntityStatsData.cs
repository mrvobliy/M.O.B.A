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
    
    [Title("Core stats", horizontalLine: true, bold: true)]
    [BoxGroup("Base stats", ShowLabel = false)]
    [SerializeField] protected int _health;
    
    [BoxGroup("Base stats", ShowLabel = false)]
    [SerializeField] protected int _armor;
    
    [BoxGroup("Base stats", ShowLabel = false)]
    [SerializeField] protected int _damage;
    
    [BoxGroup("Base stats", ShowLabel = false)]
    [SerializeField] protected float _moveSpeed;
    
    [Title("Attack stats", horizontalLine: true, bold: true)]
    [BoxGroup("Base stats", ShowLabel = false)]
    [SerializeField] protected float _attackSpeed;
    
    [BoxGroup("Base stats", ShowLabel = false)]
    [SerializeField] protected float _attackDistance;
    
    [BoxGroup("Base stats", ShowLabel = false)]
    [SerializeField] protected float _attackAngle;
    
    [BoxGroup("Base stats", ShowLabel = false)]
    [SerializeField] protected float _detectionRadius;
    
    [Title("Gold", horizontalLine: true, bold: true)]
    [BoxGroup("Costs, Experience", ShowLabel = false)]
    [SerializeField] protected int _finisherCost;
    
    [BoxGroup("Costs, Experience", ShowLabel = false)]
    [SerializeField] protected int _helpCost;
    
    [Title("Experience", horizontalLine: true, bold: true)]
    [BoxGroup("Costs, Experience", ShowLabel = false)]
    [SerializeField] protected int _experience;

    public string Name => _name;
    public int Health => _health;
    public int Armor => _armor;
    public float AttackSpeed => _attackSpeed;
    public float MoveSpeed => _moveSpeed;
    public float AttackAngle => _attackAngle;
    public float DetectionRadius => _detectionRadius;
    public int Damage => _damage;
    public float AttackDistance => _attackDistance;
    public int HelpCost => _helpCost;
    public int FinisherCost => _finisherCost;
    public int Experience => _experience;
}