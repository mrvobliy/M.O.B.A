using DG.Tweening;
using UnityEngine;

public class PlayerHealthbar : MonoBehaviour
{
    [SerializeField] private EntityHealthControl _entityHealthControl;
    [SerializeField] private RectTransform _barLine;
    [SerializeField] private Transform _scaleRoot;
    [SerializeField] private CanvasGroup _whiteFront;
    [SerializeField] private float _timeAnim;
    [SerializeField] private float _scaleValue;

    private const float StartLinePos = 0.52f;

    private void OnEnable() => _entityHealthControl.OnHealthChanged += UpdateBar;
    private void OnDisable() => _entityHealthControl.OnHealthChanged -= UpdateBar;

    private void UpdateBar()
    {
        var newLinePos = _barLine.localPosition;
        newLinePos.x = StartLinePos * (_entityHealthControl.HealthPercent - 1);
        _barLine.localPosition = newLinePos;

        PlayAnim();
    }

    private void PlayAnim()
    {
        _scaleRoot.DOScale(_scaleValue, _timeAnim).OnComplete(() =>
        {
            _scaleRoot.DOScale(1f, _timeAnim);
        });
        
        _whiteFront.DOFade(1, _timeAnim).OnComplete(() =>
        {
            _whiteFront.DOFade(0, _timeAnim);
        });
    }
}
