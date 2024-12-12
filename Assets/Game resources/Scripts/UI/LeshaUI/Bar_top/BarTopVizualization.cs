using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BarTopVizualization : MonoBehaviour
{
    [SerializeField] private TextAnimation playerTeamScore;
    [SerializeField] private TextAnimation enemyTeamScore;
    
    [SerializeField] private TMP_Text stopwatchMinutes, stopwatchSeconds;
    
    [SerializeField] private TMP_Text playerStats;

    public int hours, minutes, seconds;

    private void Start()
    {
        StarStopwatch();
    }

    private void OnEnable()
    {
        BarTopStaticManager.UpdateTeamScores += SetTeamScore;

        BarTopStaticManager.UpdatePlayerStats += SetPlayerStats;

        BarTopStaticManager.StartStopwatch += StarStopwatch;
        BarTopStaticManager.StopStopwatch  += StopStopwatch;
        BarTopStaticManager.ResetStopwatch += ResetStopwatch;
    }

    private void OnDisable()
    {
        BarTopStaticManager.UpdateTeamScores -= SetTeamScore;

        BarTopStaticManager.UpdatePlayerStats -= SetPlayerStats;
        
        BarTopStaticManager.StartStopwatch -= StarStopwatch;
        BarTopStaticManager.StopStopwatch  -= StopStopwatch;
        BarTopStaticManager.ResetStopwatch -= ResetStopwatch;
    }


    public void SetTeamScore(bool isPlayerScore, int score)
    {
        if (isPlayerScore)
        {
            playerTeamScore.ChangeText(score.ToString());
        }
        else
        {
            enemyTeamScore.ChangeText(score.ToString());
        }
    }

    public void SetStopwatchTime(int hours, int minutes, int seconds)
    {
        if (hours == 0)
        {
            stopwatchMinutes.text = minutes.ToString("00");
            stopwatchSeconds.text = seconds.ToString("00");
            //stopwatch.text = $"{minutes.ToString("00")}:{seconds.ToString("00")}";
        }
        else
        {
            stopwatchMinutes.text = minutes.ToString("00");
            stopwatchSeconds.text = seconds.ToString("00");
            //stopwatch.text = $"{hours}:{minutes.ToString("00")}:{seconds.ToString("00")}";
        }
    }

    public void SetPlayerStats(int param1, int param2, int param3)
    {
        playerStats.text = $"{param1}/{param2}/{param3}";
    }

    private Coroutine stopwatchCoroutine;

    private void StarStopwatch()
    {
        stopwatchCoroutine = StartCoroutine(Stopwatch());
    }

    private void StopStopwatch()
    {
        StopCoroutine(stopwatchCoroutine);
    }

    private void ResetStopwatch()
    {
        hours = 0;
        minutes = 0;
        seconds = 0;
    }

    IEnumerator Stopwatch()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            seconds++;

            if (seconds > 60)
            {
                seconds = 0;
                minutes++;
            }

            if (minutes > 60)
            {
                hours++;
                minutes = 0;
            }
            
            SetStopwatchTime(hours, minutes, seconds);
        }
    }
}
