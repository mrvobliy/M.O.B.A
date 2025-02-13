using DG.Tweening;
using UnityEngine;

public class ThroneHealthControl : BuildHealthControl
{
    [SerializeField] private Transform _throneCenter;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;

    protected override void PlayDestroyAnimation()
    {
        _throneCenter.DOScale(0, 0.5f).OnComplete(() =>
        {
            _explosionEffect.Play();

            foreach (var element in _rigidBodies)
            {
                element.isKinematic = false;
                element.AddExplosionForce(_explosionForce, _throneCenter.position, _explosionRadius);
            }

            Invoke(nameof(DisableComponents), DiveDelay);
        });
    }
}