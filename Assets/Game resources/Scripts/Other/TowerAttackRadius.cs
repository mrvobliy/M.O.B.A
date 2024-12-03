using System;
using DG.Tweening;
using UnityEngine;

public class TowerAttackRadius : MonoBehaviour
{
    [SerializeField] private Attacker _attacker;
    [SerializeField] private ParticleSystem _radius;
    [SerializeField] private float _timeAnim;
    [SerializeField] private float _delayToHide;

    private Renderer _renderer;
    private bool _isShowed;

    private void OnEnable()
    {
        _attacker.OnPlayerFound += Show;
        _attacker.OnPlayerLost += DelayInvokeHide;
        _renderer = _radius.GetComponent<Renderer>();
    }

    private void OnDisable()
    {
        _attacker.OnPlayerFound -= Show;
        _attacker.OnPlayerLost -= DelayInvokeHide;
    }

    private void Show()
    {
        CancelInvoke(nameof(Hide));
        _radius.gameObject.SetActive(true);
        _renderer.material.DOFade(1, _timeAnim);
    }

    private void DelayInvokeHide()
    {
        CancelInvoke(nameof(Hide));
        Invoke(nameof(Hide), _delayToHide);
    }

    private void Hide()
    {
        _isShowed = false;
        
        _renderer.material.DOFade(0, _timeAnim).OnComplete(() =>
        {
            _radius.gameObject.SetActive(false);
        });
    }
}
