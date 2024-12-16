using System;
using System.Collections;
using UnityEngine;

public class StopwatchManager : MonoBehaviour
{
    public int hours, minutes, seconds;
    private Coroutine _stopwatchCoroutine;

    public static StopwatchManager Instance;
    
    public Action StartStopwatchAction, StopStopwatchAction, ResetStopwatchAction;
    public Action<int,int> GetTime;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartStopwatchAction += StarStopwatch;
        StopStopwatchAction += StopStopwatch;
        ResetStopwatchAction += ResetStopwatch;
        
        StarStopwatch();
    }
    
    private void StarStopwatch()
    {
        _stopwatchCoroutine = StartCoroutine(Stopwatch());
    }

    private void StopStopwatch()
    {
        StopCoroutine(_stopwatchCoroutine);
    }

    private void ResetStopwatch()
    {
        hours = 0;
        minutes = 0;
        seconds = 0;
        
        GetTime?.Invoke(0, 0);
    }
    
    private IEnumerator Stopwatch()
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
            
            GetTime?.Invoke(minutes, seconds);
        }
    }
}
