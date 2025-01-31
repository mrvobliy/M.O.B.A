using System;
using UnityEngine;

public class HeroExperienceControl : MonoBehaviour
{
    public event Action OnExperienceChanged;
    public int Level { get; private set; }

    private int _experience;
    
    public void SetExperience(int value)
    {
        _experience += value;
        OnExperienceChanged?.Invoke();
    }
}