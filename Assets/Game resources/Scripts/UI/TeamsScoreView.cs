using Sirenix.OdinInspector;
using UnityEngine;

public class TeamsScoreView : MonoBehaviour
{
    [SerializeField] private TeamsScoreTextAnimation _lightSideScore;
    [SerializeField] private TeamsScoreTextAnimation _darkSideScore;
    
    [Button]
    public void SetTeamScore(bool isTeamLight, int score)
    {
        if (isTeamLight)
            _lightSideScore.ChangeText(score.ToString());
        else
            _darkSideScore.ChangeText(score.ToString());
    }
}
