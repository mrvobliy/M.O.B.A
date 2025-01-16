using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AttackEffectControl : MonoBehaviour
{
    [SerializeField] private Attacker _attacker;
    [SerializeField] private AnimationEvents _animationEvents;
    [SerializeField] private List<ParticleSystem> _attackEffects;
    [SerializeField] private List<ParticleSystem> _hitEnemyEffects;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private bool _isRandomAttackEffect;
    [SerializeField] private bool _isRandomEnenmyHitEffect;
    
    private void OnEnable()
    {
        _attacker.OnTargetHit += PlayEnemyHitEffect;
        _animationEvents.OnPlayAttackEffect += PlayAttackEffect;
    }

    private void OnDisable()
    {
        _attacker.OnTargetHit -= PlayEnemyHitEffect;
        _animationEvents.OnPlayAttackEffect -= PlayAttackEffect;
    }

    private void PlayAttackEffect()
    {
        if (_isRandomAttackEffect)
        {
            var index = Random.Range(0, _attackEffects.Count);
            Instantiate(_attackEffects[index], _spawnPoint.position, _spawnPoint.rotation);
        }
    }
    
    private void PlayEnemyHitEffect(Target target)
    {
        if (_isRandomEnenmyHitEffect)
        {
            var index = Random.Range(0, _hitEnemyEffects.Count);
            Instantiate(_hitEnemyEffects[index], target.GetAttackPoint().position, Quaternion.identity);
        }
    }
}
