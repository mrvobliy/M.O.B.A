using System.Collections;
using TMPro;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _minutesText;
    [SerializeField] private TMP_Text _secondsText;

    private int _minutes;
    private int _seconds;
    private Coroutine _onTimePlayCoroutine;
    
    private void Start() => _onTimePlayCoroutine = StartCoroutine(OnTimePlay());

    private void StopStopwatch() => StopCoroutine(_onTimePlayCoroutine);

    private void SetTimeView()
    {
        _minutesText.text = _minutes.ToString("00");
        _secondsText.text = _seconds.ToString("00");
    }
    
    private IEnumerator OnTimePlay()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            _seconds++;

            if (_seconds > 60)
            {
                _seconds = 0;
                _minutes++;
            }

            SetTimeView();
        }
    }
}
