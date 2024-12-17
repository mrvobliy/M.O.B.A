using TMPro;
using UnityEngine;

public class BarTopVizualization : MonoBehaviour
{
    [SerializeField] private TextAnimation _playerTeamScore;
    [SerializeField] private TextAnimation _enemyTeamScore;
    
    [SerializeField] private TMP_Text _stopwatchMinutes;
    [SerializeField] private TMP_Text _stopwatchSeconds;

    [SerializeField] private TMP_Text _playerStats;

    public static BarTopVizualization Instance;
    
    private static int[] _playerstats = { 0, 0, 0 };

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StopwatchManager.Instance.GetTime += SetStopwatchTime;
    }

    public void SetPlayerStats(PlayerStats stats, int count)
    {
        _playerstats[stats.GetHashCode()] = count;
        _playerStats.text = $"{_playerstats[0]}/{_playerstats[1]}/{_playerstats[2]}";
    }
    
    public void SetTeamScore(bool isPlayerScore, int score)
    {
        if (isPlayerScore)
        {
            _playerTeamScore.ChangeText(score.ToString());
        }
        else
        {
            _enemyTeamScore.ChangeText(score.ToString());
        }
    }

    public void SetStopwatchTime(int minutes, int seconds)
    {
        _stopwatchMinutes.text = minutes.ToString("00");
        _stopwatchSeconds.text = seconds.ToString("00");
    }
}
