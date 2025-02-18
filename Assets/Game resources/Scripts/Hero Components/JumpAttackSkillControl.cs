using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class JumpAttackSkillControl : MonoBehaviour
{
    private const float BlendAttackLayerDuration = 0.3f;
    
    [SerializeField] private EntityComponentsData _entityData;
    [SerializeField] private RectTransform _skillButton;
    [SerializeField] private ButtonEvents _skillButtonEvents;
    [SerializeField] private DecalProjector _indicator;
    [SerializeField] private float _sensitivity = 0.1f;
    [SerializeField] private float _fadeSpeed = 2.0f;
    [Space]
    [SerializeField] private Transform _destinationPoint;
    [SerializeField] private HeroSkillDamage _damagePrefab;
    [SerializeField] private Transform _skillSpawnPoint;
    
    private Vector3 _previousToMouseDir;
    private Coroutine _fadeCoroutine;
    private float _startSpeed;

    private void OnEnable()
    {
        if (_entityData.IsAi)
        {
            gameObject.SetActive(false);
            return;
        }
        
        _skillButtonEvents.OnButtonDown += StartSkill;
        _skillButtonEvents.OnButtonUp += ReleaseSkill;
        _skillButtonEvents.OnButtonDrag += RotateIndicator;
    }

    private void OnDisable()
    {
        _skillButtonEvents.OnButtonDown -= StartSkill;
        _skillButtonEvents.OnButtonUp -= ReleaseSkill;
        _skillButtonEvents.OnButtonDrag -= RotateIndicator;
    }

    private void StartSkill()
    {
        StartFade(0.3f);
        RotateIndicator();
    }
    
    private void ReleaseSkill()
    {
        StartFade(0.0f);
        
        if (!_entityData.CanComponentsWork || _entityData.IsDead) return;
		
        _entityData.Animator.SetTrigger(AnimatorHash.IsFirstSkill);
        _entityData.Animator.DOLayerWeight(4, 1f, BlendAttackLayerDuration);
        _entityData.SetComponentsWorkState(false);
        
        var toRotation = Quaternion.LookRotation(_destinationPoint.position - _entityData.RotationRoot.position, Vector3.up);
        _entityData.RotationRoot.DORotate(toRotation.eulerAngles, 0.2f);

        StartCoroutine(OnActivate());
		
        IEnumerator OnActivate()
        {
            _startSpeed = _entityData.NavMeshAgent.speed;
            _entityData.NavMeshAgent.speed = 4;
            _entityData.NavMeshAgent.SetDestination(_destinationPoint.position);
			
            yield return new WaitForSeconds(0.8f);

            var skillDamage = Instantiate(_damagePrefab, _skillSpawnPoint);
            skillDamage.Init(_entityData);
            skillDamage.gameObject.SetActive(true);
            skillDamage.gameObject.transform.SetParent(null);
			
            yield return new WaitForSeconds(2);
            
            _entityData.NavMeshAgent.speed = _startSpeed;
            _entityData.Animator.DOLayerWeight(4, 0f, BlendAttackLayerDuration);
            _entityData.SetComponentsWorkState(true);
        }
    }

    private void RotateIndicator()
    {
        var toMouseDir = Input.mousePosition - _skillButton.position;
        var angle = Vector3.SignedAngle(_skillButton.up, toMouseDir, -_skillButton.forward);
        _indicator.transform.parent.localRotation = Quaternion.Euler(90, angle, -180);
    }

    private void StartFade(float targetFade)
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);
        
        _fadeCoroutine = StartCoroutine(FadeIndicator(targetFade));
    }

    private IEnumerator FadeIndicator(float targetFade)
    {
        while (Mathf.Abs(_indicator.fadeFactor - targetFade) > 0.01f)
        {
            _indicator.fadeFactor = Mathf.Lerp(_indicator.fadeFactor, targetFade, Time.deltaTime * _fadeSpeed);
            yield return null;
        }
        
        _indicator.fadeFactor = targetFade;
    }
}