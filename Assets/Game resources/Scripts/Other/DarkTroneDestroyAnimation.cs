using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class DarkTroneDestroyAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider _collider;
    [SerializeField] private Target _target;
    [SerializeField] private List<Rigidbody> _elements;
    [SerializeField] private Transform _center;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;
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

        sequence.Append(_center.DOScale(0, 0.3f));
        sequence.AppendCallback(() =>
        {
            _collider.enabled = false;
            _animator.SetTrigger(AnimatorHash.ThroneDeath);
            _exploseEffect.Play();

            foreach (var element in _elements)
            {
                element.isKinematic = false;

                var dir = element.position - _center.position;
                element.AddExplosionForce(_explosionForce, _center.position, _explosionRadius);
            }
        });
        sequence.AppendInterval(2);
        sequence.AppendCallback(() => 
        {
            foreach (var element in _elements)
                element.isKinematic = true;
        });
    }
}
