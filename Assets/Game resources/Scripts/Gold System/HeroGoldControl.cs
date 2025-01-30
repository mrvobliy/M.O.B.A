using System;
using UnityEngine;

public class HeroGoldControl : MonoBehaviour
{
    [SerializeField] private EntityComponentsData _componentsData;
    [SerializeField] private int _goldBalance;

    public event Action<int> OnBalanceChanged;

    private void Start()
    {
        if (!_componentsData.IsAi)
            PlayerGoldBalanceView.Instance.SetHeroGoldControl(this);

        EventsBase.GetGoldForTeam += GetGoldForTeam;
    }

    private void OnDisable() => EventsBase.GetGoldForTeam -= GetGoldForTeam;

    public void SetGold(int value)
    {
        _goldBalance += value;
        OnBalanceChanged?.Invoke(_goldBalance);
    }

    private void GetGoldForTeam(int value, Team team)
    {
        if (_componentsData.EntityTeam != team) return;
        
        SetGold(value);
    }
}