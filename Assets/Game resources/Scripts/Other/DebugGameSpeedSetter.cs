using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugGameSpeedSetter : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _image;
    
    private int _timeScale = 1;

    public void SetSpeed()
    {
        if (_timeScale == 1)
        {
            _timeScale = 3;
            Time.timeScale = _timeScale;
            
            _text.text = "3x game speed";
            _image.color = new Color(1, 0.124f, 0, 0.5f);
        }
        else
        {
            _timeScale = 1;
            Time.timeScale = _timeScale;
            
            _text.text = "1x game speed";
            _image.color = new Color(0, 1, 0.09f, 0.5f);
        }
    }
}