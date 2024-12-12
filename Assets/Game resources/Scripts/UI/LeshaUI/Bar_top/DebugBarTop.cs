using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugBarTop : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputFieldKills;
    [SerializeField] private TMP_InputField _inputFieldDeath;
    [SerializeField] private TMP_InputField _inputFieldSupport;
    
    [SerializeField] private TMP_InputField _inputFieldTeamPlayer;
    [SerializeField] private TMP_InputField _inputFieldTeamEnemy;
    
    
    public void StartStopwatch()
    {
        BarTopStaticManager.StartStopwatch?.Invoke();
    }
    
    public void StopStopwatch()
    {
        BarTopStaticManager.StopStopwatch?.Invoke();
    }
    
    public void ResetStopwatch()
    {
        BarTopStaticManager.ResetStopwatch?.Invoke();
    }
    
    
    public void SetKills()
    {
        if(string.IsNullOrEmpty(_inputFieldKills.text)) return;
        BarTopStaticManager.SetPlayerStats(BarTopStaticManager.PlayerStats.kills, Convert.ToInt32(_inputFieldKills.text));
    }
    
    public void SetDeath()
    {
        if(string.IsNullOrEmpty(_inputFieldDeath.text)) return;
        BarTopStaticManager.SetPlayerStats(BarTopStaticManager.PlayerStats.death, Convert.ToInt32(_inputFieldDeath.text));
    }
    
    public void SetSupport()
    {
        if(string.IsNullOrEmpty(_inputFieldSupport.text)) return;
        BarTopStaticManager.SetPlayerStats(BarTopStaticManager.PlayerStats.supports, Convert.ToInt32(_inputFieldSupport.text));
    }

    public void SetTeamPlayerScore()
    {
        if(string.IsNullOrEmpty(_inputFieldTeamPlayer.text)) return;
        BarTopStaticManager.SetPlayerTeamScore(Convert.ToInt32(_inputFieldTeamPlayer.text));
    }
    
    public void SetTeamEnemyScore()
    {
        if(string.IsNullOrEmpty(_inputFieldTeamEnemy.text)) return;
        BarTopStaticManager.SetEnemyTeamScore(Convert.ToInt32(_inputFieldTeamEnemy.text));
    }
}
