using UnityEngine;

public class EntityStatsControl : MonoBehaviour
{
    [SerializeField] private string _name;
    
    public int Health;
    public int Armor;
    public int AttackSpeed;
    public int MoveSpeed;
    public int Damage;
    public int AttackDistance;

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