using System.Collections;
using UnityEngine;

public class FuryAttackSkillControl : MonoBehaviour
{
    private const float BlendAttackLayerDuration = 0.3f;
    
    [SerializeField] private EntityComponentsData _entityComponentsData;
    [SerializeField] private ButtonEvents _skillButtonEvents;
    [SerializeField] private PlayerSkillDamage _damagePrefab;
    [SerializeField] private Transform _spawnPoint;

    private void OnEnable() => _skillButtonEvents.OnButtonDown += ReleaseSkill;

    private void OnDisable() => _skillButtonEvents.OnButtonDown -= ReleaseSkill;

    private void ReleaseSkill()
    {
        if (!_entityComponentsData.CanComponentsWork) return;
        
        StartCoroutine(OnActivate());
        return;

        IEnumerator OnActivate()
        {
            _entityComponentsData.EntityHealthControl.Animator.SetTrigger(AnimatorHash.IsSecondSkill);
            _entityComponentsData.EntityHealthControl.Animator.DOLayerWeight(4, 1f, BlendAttackLayerDuration);
            _entityComponentsData.SetWorkState(false);
            
            yield return new WaitForSeconds(0.3f);
			
            var skillDamage = Instantiate(_damagePrefab, _spawnPoint);
            skillDamage.Init(_entityComponentsData);
            skillDamage.gameObject.SetActive(true);
            skillDamage.gameObject.transform.SetParent(null);
			
            yield return new WaitForSeconds(1f);
            
            _entityComponentsData.EntityHealthControl.Animator.DOLayerWeight(5, 0f, BlendAttackLayerDuration);
            _entityComponentsData.SetWorkState(true);
        }
    }
}