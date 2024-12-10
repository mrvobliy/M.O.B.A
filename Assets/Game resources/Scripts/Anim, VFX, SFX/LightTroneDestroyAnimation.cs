using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class LightTroneDestroyAnimation :MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider _collider;
    [SerializeField] private Target _target;
    [SerializeField] private List<Rigidbody> _elements;
    [SerializeField] private Transform _center;
    [SerializeField] private float _explosionForce;
    [SerializeField] private ParticleSystem _exploseEffect;

    private void OnEnable()
    {
        _target.OnDeath += PlayAnimation;
    }

    private void OnDisable()
    {
        _target.OnDeath -= PlayAnimation;
    }

    [ProButton]
    private void PlayAnimation()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(_center.DOScale(0, 1f));
        sequence.AppendCallback(() =>
        {
            _collider.enabled = false;
            _animator.SetTrigger(AnimatorHash.ThroneDeath);
            _exploseEffect.Play();

            /*foreach (var element in _elements)
            {
                element.isKinematic = false;
                element.AddForce(Vector3.up * _explosionForce);
            }*/
        });
        /*sequence.AppendInterval(2);
        sequence.AppendCallback(() =>
        {
            foreach (var element in _elements)
                element.isKinematic = true;
        });*/
    }
}
