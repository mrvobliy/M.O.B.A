using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class DeathRespawnControl : MonoBehaviour
{
    [SerializeField] private Target _target;
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _diveDelay = 3f;
    [SerializeField] private float _diveDuration = 10f;
    [SerializeField] private float _diveDepth = 1f;

    private void OnEnable()
    {
        _target.OnOverrideDeath += PlayDeathAnimation;
    }

    private void OnDisable()
    {
        _target.OnOverrideDeath -= PlayDeathAnimation;
    }

    private void PlayDeathAnimation()
    {
        var sequence = DOTween.Sequence();
        
        sequence.AppendCallback(() =>
        {
            _agent.enabled = false;
            _animator.SetLayerWeight(6, 1);
            _animator.SetBool(AnimatorHash.IsDeath, true);
        });
        sequence.AppendInterval(_diveDelay);
        sequence.Append(transform.DOLocalMoveY(transform.localPosition.y - _diveDepth, _diveDuration));
        sequence.AppendCallback(Respawn); 
    }

    private void Respawn()
    {
        _animator.SetLayerWeight(6, 0);
        _animator.SetBool(AnimatorHash.IsDeath, false);
        transform.position = HeroSpawnControl.Instance.GetPoint(_target.Team).position;
        _target.RotationParent.rotation = HeroSpawnControl.Instance.GetPoint(_target.Team).rotation;
        _agent.enabled = true;
        _target.RestoreFullHeath();
    }
}
