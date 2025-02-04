using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class HeroExperienceControl : MonoBehaviour
{
    [SerializeField] private ExperienceForLevelsData _levelsData;
    [SerializeField] private int _currentExperience;
    [SerializeField] private int _currentLevelExperience;
    [SerializeField] private int _nextLevelExperience = 1;
    [SerializeField] private float _experienceAttitude;
    
    public event Action<float> OnExperienceChanged;
    public event Action<int> OnLevelChanged;
    
    public int Level { get; private set; }
    
    
    private void Start()
    {
        Level = 1;
        _nextLevelExperience =  _levelsData.GetNextLevelExperience(Level);
        _experienceAttitude = Mathf.InverseLerp(_currentLevelExperience, _nextLevelExperience, _currentExperience);
        OnExperienceChanged?.Invoke(_experienceAttitude);
        OnLevelChanged?.Invoke(Level);
    }

    [Button]
    public void SetExperience(int value)
    {
        if (Level >= 20) return;
        
        _currentExperience += value;
        _experienceAttitude = Mathf.InverseLerp(_currentLevelExperience, _nextLevelExperience, _currentExperience);
        OnExperienceChanged?.Invoke(_experienceAttitude);
        TryUpdateLevel();
    }

    private void TryUpdateLevel()
    {
        if (Level >= 20) return;
        
        var newLevel = _levelsData.GetLevel(_currentExperience);
        
        if (newLevel == Level) return;

        Level = newLevel;
        _currentLevelExperience = _levelsData.GetNextLevelExperience(Level - 1);
        _nextLevelExperience = _levelsData.GetNextLevelExperience(Level);
        _experienceAttitude = Mathf.InverseLerp(_currentLevelExperience, _nextLevelExperience, _currentExperience);
        OnExperienceChanged?.Invoke(_experienceAttitude);
        OnLevelChanged?.Invoke(Level);
    }
}