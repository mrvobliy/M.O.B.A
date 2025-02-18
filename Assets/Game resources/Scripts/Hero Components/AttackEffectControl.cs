using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AttackEffectControl : MonoBehaviour
{
    [SerializeField] private HeroAttackControl _heroAttackControl;
    [SerializeField] private AnimationEvents _animationEvents;
    [SerializeField] private List<ParticleSystem> _attackEffects;
    [SerializeField] private List<ParticleSystem> _hitEnemyEffects;
    [SerializeField] private Transform _spawnPoint;
    
    private void OnEnable()
    {
        _heroAttackControl.OnTargetsHit += PlayEnemyHitEffect;
        _animationEvents.OnPlayAttackEffect += PlayAttackEffect;
    }

    private void OnDisable()
    {
        _heroAttackControl.OnTargetsHit -= PlayEnemyHitEffect;
        _animationEvents.OnPlayAttackEffect -= PlayAttackEffect;
    }

    private void PlayAttackEffect()
    {
        var index = Random.Range(0, _attackEffects.Count);
        Instantiate(_attackEffects[index], _spawnPoint.position, _spawnPoint.rotation);
    }
    
    private void PlayEnemyHitEffect(List<EntityComponentsData> enemies)
    {
        foreach (var enemy in enemies)
        {
            var index = Random.Range(0, _hitEnemyEffects.Count);
            Instantiate(_hitEnemyEffects[index], enemy.EnemyAttackPoint.position, Quaternion.identity);
        }
    }
}