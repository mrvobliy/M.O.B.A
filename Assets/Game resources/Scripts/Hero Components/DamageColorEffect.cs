using System;
using System.Collections.Generic;
using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening;
using UnityEngine;

public class DamageColorEffect : MonoBehaviour
{
    [SerializeField] private EntityHealthControl _entityHealthControl;
    
    [SerializeField] private List<Material> _materials;
    [SerializeField] private List<string> _exludeMaterials;
    [SerializeField] private List<Renderer> _renderers;
    [SerializeField] private Color _startColor;
    [SerializeField] private Color _damageColor;
    [SerializeField] private float _timeChangeColor;
    [SerializeField] private bool _isPlayer;

    private void OnEnable()
    {
        _entityHealthControl.OnEnemyAttackUs += ChangeColor;
        CloneMaterials();
    }

    private void OnDisable()
    {
        _entityHealthControl.OnEnemyAttackUs -= ChangeColor;

        foreach (var mat in _materials)
        {
            mat.SetColor("_EmissionColor", _startColor);
        }
    }

    [ProButton]
    private void ChangeColor(EntityComponentsData target, int damage)
    {
        if (!target.transform.CompareTag("Player") && !_isPlayer) return;
        
        foreach (var mat in _materials)
        {
            if (!mat.HasColor("_EmissionColor")) continue;
            
            mat.DOColor(_damageColor, "_EmissionColor",_timeChangeColor).OnComplete(() =>
            {
                mat.DOColor(_startColor, "_EmissionColor", _timeChangeColor);
            });
        }
    }

    private void CloneMaterials()
    {
        foreach (var meshRenderer in _renderers)
        {
            var materials = new List<Material>();
            meshRenderer.GetMaterials(materials);

            foreach (var material in materials)
            {
                if (_exludeMaterials.Contains(material.name.Replace(" (Instance)", ""))) continue;
                
                _materials.Add(material);
            }
        }
    }
}
