using System;
using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening;
using UnityEngine;

public class PlayerHealthbar : MonoBehaviour
{
    [SerializeField] private Target _target;
    [SerializeField] private RectTransform _barLine;
    [SerializeField] private Transform _scaleRoot;
    [SerializeField] private CanvasGroup _whiteFront;
    [SerializeField] private float _timeAnim;
    [SerializeField] private float _scaleValue;

    private float _startLinePos = 0.52f;
    
    private void OnEnable()
    {
        _target.OnDamageTaken += UpdateBar;
    }

    private void OnDisable()
    {
        _target.OnDamageTaken -= UpdateBar;
    }

    private void UpdateBar()
    {
        var newLinePos = new Vector3();
        newLinePos = _barLine.localPosition;
        newLinePos.x = _startLinePos * (_target.HealthPercent - 1);
        _barLine.localPosition = newLinePos;

        PlayAnim();
    }

    [ProButton]
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
