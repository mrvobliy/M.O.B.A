using System;
using System.Collections.Generic;
using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening;
using UnityEngine;

public class PlayerDamageEffect : MonoBehaviour
{
    [SerializeField] private Player _player;
    
    [SerializeField] private List<Material> _materials;
    [SerializeField] private Color _startColor;
    [SerializeField] private Color _damageColor;
    [SerializeField] private float _timeChangeColor;

    private void OnEnable()
    {
        _player.OnDamageTaken += ChangeColor;
    }

    private void OnDisable()
    {
        _player.OnDamageTaken -= ChangeColor;
        
        foreach (var mat in _materials)
            mat.color = _startColor;
    }

    [ProButton]
    private void ChangeColor()
    {
        foreach (var mat in _materials)
        {
            mat.DOColor(_damageColor, _timeChangeColor).OnComplete(() =>
            {
                mat.DOColor(_startColor, _timeChangeColor);
            });
        }
    }
}
