using System.Collections;
using UnityEngine;

public class FuryAttackSkillControl : MonoBehaviour
{
    private const float BlendAttackLayerDuration = 0.3f;
    
    [SerializeField] private EntityComponentsData _entityData;
    [SerializeField] private HeroSkillView _heroSkillView;
    [SerializeField] private DoubleClickButton _doubleClickButton;
    [SerializeField] private ButtonEvents _buttonEvents;
    [SerializeField] private HeroSkillDamage _damagePrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private int _cooldown;

    private bool _isSkillWork;
    private bool _isCooldown;

    private void OnEnable()
    {
        if (_entityData.IsAi)
        {
            gameObject.SetActive(false);
            return;
        }
        
        _doubleClickButton.OnDoubleClick += ReleaseSkill;
        _buttonEvents.OnButtonUp += ReleaseSkill;
    }

    private void OnDisable()
    {
        _doubleClickButton.OnDoubleClick -= ReleaseSkill;
        _buttonEvents.OnButtonUp -= ReleaseSkill;
    }

    private void ReleaseSkill()
    {
        if (_isCooldown) return;
        
        if (!_entityData.CanComponentsWork || _entityData.IsDead) return;
        
        StartCoroutine(OnActivate());
        return;

        IEnumerator OnActivate()
        {
            _isCooldown = true;
            _heroSkillView.PlayCooldownAnim(_cooldown);
            Invoke(nameof(ResetCooldown), _cooldown);
            
            _entityData.Animator.SetTrigger(AnimatorHash.IsSecondSkill);
            _entityData.Animator.DOLayerWeight(4, 1f, BlendAttackLayerDuration);
            _entityData.SetComponentsWorkState(false);
            
            yield return new WaitForSeconds(0.3f);
			
            var skillDamage = Instantiate(_damagePrefab, _spawnPoint);
            skillDamage.Init(_entityData);
            skillDamage.gameObject.SetActive(true);
            skillDamage.gameObject.transform.SetParent(null);
			
            yield return new WaitForSeconds(1f);
            
            _entityData.Animator.DOLayerWeight(5, 0f, BlendAttackLayerDuration);
            _entityData.SetComponentsWorkState(true);
        }
    }
    
    private void ResetCooldown() => _isCooldown = false;
}