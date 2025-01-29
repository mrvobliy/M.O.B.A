using System;
using UnityEngine;

public class HeroGoldControl : MonoBehaviour
{
    [SerializeField] private int _goldBalance;

    public event Action OnBalanceChanged;

    public int GoldBalance => _goldBalance;

    public void SetGold(int value)
    {
        _goldBalance += value;
        OnBalanceChanged?.Invoke();
    }
}