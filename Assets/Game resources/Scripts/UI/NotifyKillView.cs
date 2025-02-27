using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NotifyKillView : MonoBehaviour
{
    private const float TimeShowHide = 0.2f;
    private const float TimeWait = 1.5f;
    
    [SerializeField] private CanvasGroup _canvasGroup;
    [Space]
    [SerializeField] private Image _killerImage;
    [SerializeField] private Image _deadImage;
    [SerializeField] private Image _killTypeImage;
    [Space]
    [SerializeField] private GameObject _alliesBg;
    [SerializeField] private GameObject _enemyBg;
    [Space] 
    [SerializeField] private Vector3 _startPos;

    private Action _callback;

    public bool IsBusy { get; private set; }
    
    public void SetInfo(EntityComponentsData killer, EntityComponentsData dead)
    {
        _killerImage.sprite = killer.HeroInfo.Avatar;
        _killerImage.sprite = dead.HeroInfo.Avatar;
        _enemyBg.SetActive(killer.EntityTeam != VariableBase.PlayerTeam);
    }

    public void SetShowCallback(Action callback) => _callback = callback;
    
    public void Show()
    {
        IsBusy = true;
        
        var seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMoveX(0f, TimeShowHide));
        seq.Join(_canvasGroup.DOFade(1, TimeShowHide));
        seq.AppendInterval(TimeWait);
        seq.AppendCallback(Hide);
    }

    private void Hide()
    {
        var seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMoveX(-500f, TimeShowHide));
        seq.Join(_canvasGroup.DOFade(0, TimeShowHide));
        seq.AppendCallback(() =>
        {
            IsBusy = false;
            _callback?.Invoke();
        });
    }
}