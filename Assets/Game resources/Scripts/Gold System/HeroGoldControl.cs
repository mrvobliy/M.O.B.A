using System;
using UnityEngine;

public class HeroGoldControl : MonoBehaviour
{
    [SerializeField] private ParticleSystem _goldEffect;
    [SerializeField] private int _goldBalance;

    public event Action OnBalanceChanged;

    public int GoldBalance => _goldBalance;
    
    private void OnEnable()
    {
        EventsBase.EntityDeath += TryGetGold;
    }

    private void OnDisable()
    {
        EventsBase.EntityDeath -= TryGetGold;
    }

    private void TryGetGold(EntityComponentsData componentsData)
    {
        if (componentsData.HeroGoldControl != this) return;

        _goldBalance += GoldManager.Instance.GetCostForEntity(componentsData);
        OnBalanceChanged?.Invoke();
    }
}
