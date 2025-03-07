using Sirenix.OdinInspector;
using UnityEngine;

public class EntityStatsControl : MonoBehaviour
{
    [SerializeField] private string _name;
    
    [ReadOnly]
    public int Health;
    [ReadOnly]
    public int Armor;
    [ReadOnly]
    public float AttackSpeed;
    [ReadOnly]
    public float MoveSpeed;
    [ReadOnly]
    public int Damage;
    [ReadOnly]
    public float AttackDistance;

    private EntityStatsData _statsData;

    protected virtual void Awake()
    {
        _statsData = GameDataManager.Instance.DataBase.GetEntityStatsData(_name);
        
        if (_statsData == null) return;
        SetStats();
    }

    protected virtual void SetStats()
    {
        Health = _statsData.Health;
        Armor = _statsData.Armor;
        AttackSpeed = _statsData.AttackSpeed;
        MoveSpeed = _statsData.MoveSpeed;
        Damage = _statsData.Damage;
        AttackDistance = _statsData.AttackDistance;
    }
}