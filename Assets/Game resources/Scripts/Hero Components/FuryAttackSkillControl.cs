using System.Collections;
using UnityEngine;

public class FuryAttackSkillControl : MonoBehaviour
{
    private const float BlendAttackLayerDuration = 0.3f;
    
    [SerializeField] private EntityComponentsData _entityData;
    [SerializeField] private ButtonEvents _skillButtonEvents;
    [SerializeField] private HeroSkillDamage _damagePrefab;
    [SerializeField] private Transform _spawnPoint;

    private void OnEnable() => _skillButtonEvents.OnButtonDown += ReleaseSkill;

    private void OnDisable() => _skillButtonEvents.OnButtonDown -= ReleaseSkill;

    private void ReleaseSkill()
    {
        if (!_entityData.CanComponentsWork || _entityData.IsDead) return;
        
        StartCoroutine(OnActivate());
        return;

        IEnumerator OnActivate()
        {
            _entityData.Animator.SetTrigger(AnimatorHash.IsSecondSkill);
            _entityData.Animator.DOLayerWeight(4, 1f, BlendAttackLayerDuration);
            _entityData.SetWorkState(false);
            
            yield return new WaitForSeconds(0.3f);
			
            var skillDamage = Instantiate(_damagePrefab, _spawnPoint);
            skillDamage.Init(_entityData);
            skillDamage.gameObject.SetActive(true);
            skillDamage.gameObject.transform.SetParent(null);
			
            yield return new WaitForSeconds(1f);
            
            _entityData.Animator.DOLayerWeight(5, 0f, BlendAttackLayerDuration);
            _entityData.SetWorkState(true);
        }
    }
}