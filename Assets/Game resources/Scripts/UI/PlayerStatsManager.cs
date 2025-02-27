using TMPro;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private int _kills;
    private int _deaths;
    private int _supports;
    
    public void UpdateView()
    {
        _text.text = _kills + "/" + _deaths + "/" + _supports;
    }
}
