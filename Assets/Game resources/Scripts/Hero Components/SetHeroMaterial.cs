using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SetHeroMaterial : MonoBehaviour
{
    [SerializeField] private EntityComponentsData _componentsData;
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;
    [SerializeField] private Material _lightMaterial;
    [SerializeField] private Material _darkMaterial;

    [Button]
    private void SetMaterial()
    {
        var material = _componentsData.EntityTeam == Team.Light ? _lightMaterial : _darkMaterial;
        var materials = new List<Material>();
        _meshRenderer.GetMaterials(materials);
        materials[0] = material;
        _meshRenderer.SetMaterials(materials);
    }

    private void Awake() => SetMaterial();
    private void OnDisable() => SetMaterial();
}