using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu]
public class ExperienceForLevelsData : ScriptableObject
{
    [TableList]
    [SerializeField] private List<ExperienceForLevel> _experienceForLevels;

    public int GetLevel(int experience)
    {
        for (var i = 1; i < _experienceForLevels.Count; i++)
        {
            if (_experienceForLevels[i - 1].TotalExperience <= experience && _experienceForLevels[i].TotalExperience > experience)
                return _experienceForLevels[i - 1].Level;
        }
        
        return _experienceForLevels[^1].Level;
    }

    public int GetNextLevelExperience(int level)
    {
        return level + 1 > _experienceForLevels.Count ? _experienceForLevels[^1].TotalExperience 
            : _experienceForLevels[level].TotalExperience;
    }

    [Button]
    public void SetLevels()
    {
        for (var i = 0; i < _experienceForLevels.Count; i++) 
            _experienceForLevels[i].Level = i + 1;
    }
    
    [Button]
    public void SetExperience()
    {
        for (var i = 1; i < _experienceForLevels.Count; i++)
        {
            var last =  _experienceForLevels[i - 1];
            var current =  _experienceForLevels[i];

            current.NeedExperience = last.NeedExperience + current.CorrectionalCoeffcient * current.Level;
            current.TotalExperience = last.NeedExperience + current.NeedExperience;
        } 
    }
}

[Serializable]
public class ExperienceForLevel 
{
    public int Level;
    public int NeedExperience;
    public int TotalExperience;
    public int CorrectionalCoeffcient = 1;
}