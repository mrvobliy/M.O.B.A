using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BarTopStaticManager
{
    public enum PlayerStats
    {
        kills = 0,
        death = 1,
        supports = 2,
    }
    
    public static Action<int, int, int> UpdatePlayerStats;
    
    public static Action<bool,int> UpdateTeamScores;

    public static Action StartStopwatch, StopStopwatch, ResetStopwatch;


    private static int[] playerstats = new[] { 0, 0, 0, };

    public static void SetPlayerStats(PlayerStats stats, int count)
    {
        playerstats[stats.GetHashCode()] = count;
        UpdatePlayerStats?.Invoke(playerstats[0], playerstats[1], playerstats[2]);
    }

    public static void SetPlayerTeamScore(int score)
    {
        UpdateTeamScores?.Invoke(true,score);
    }
    
    public static void SetEnemyTeamScore(int score)
    {
        UpdateTeamScores?.Invoke(false,score);
    }
}