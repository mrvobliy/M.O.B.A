using System;
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
        StopwatchManager.Instance.StartStopwatchAction?.Invoke();
    }
    
    public void StopStopwatch()
    {
        StopwatchManager.Instance.StopStopwatchAction?.Invoke();
    }
    
    public void ResetStopwatch()
    {
        StopwatchManager.Instance.ResetStopwatchAction?.Invoke();
    }
    
    public void SetKills()
    {
        if(string.IsNullOrEmpty(_inputFieldKills.text)) return;
        
        BarTopVizualization.Instance.SetPlayerStats(PlayerStats.Kills,Convert.ToInt32(_inputFieldKills.text));
    }
    
    public void SetDeath()
    {
        if(string.IsNullOrEmpty(_inputFieldDeath.text)) return;
        
        BarTopVizualization.Instance.SetPlayerStats(PlayerStats.Death,Convert.ToInt32(_inputFieldDeath.text));
    }
    
    public void SetSupport()
    {
        if(string.IsNullOrEmpty(_inputFieldSupport.text)) return;
        
        BarTopVizualization.Instance.SetPlayerStats(PlayerStats.Supports,Convert.ToInt32(_inputFieldSupport.text));
    }

    public void SetTeamPlayerScore()
    {
        if(string.IsNullOrEmpty(_inputFieldTeamPlayer.text)) return;
        
        BarTopVizualization.Instance.SetTeamScore(true, Convert.ToInt32(_inputFieldTeamPlayer.text));
    }
    
    public void SetTeamEnemyScore()
    {
        if(string.IsNullOrEmpty(_inputFieldTeamEnemy.text)) return;
        
        BarTopVizualization.Instance.SetTeamScore(false, Convert.ToInt32(_inputFieldTeamEnemy.text));
    }
}
