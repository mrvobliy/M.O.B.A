using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class JumpAttackSkillControl : MonoBehaviour
{
    private const float BlendAttackLayerDuration = 0.3f;
    
    [SerializeField] private EntityComponentsData _componentsData;
    [SerializeField] private Collider _collider;
    [SerializeField] private Collider _excludeCollider;
    [SerializeField] private RectTransform _skillButton;
    [SerializeField] private ButtonEvents _skillButtonEvents;
    [SerializeField] private DecalProjector _indicator;
    [Space]
    [SerializeField] private float _sensitivity = 0.1f;
    [SerializeField] private float _fadeSpeed = 2.0f;
    [Space]
    [SerializeField] private Transform _destinationPoint;
    [SerializeField] private HeroSkillDamage _damagePrefab;
    [SerializeField] private Transform _skillSpawnPoint;
    [Space] 
    [SerializeField] private float _speedMove;
    [SerializeField] private float _timeMove;
    
    private Vector3 _previousToMouseDir;
    private Coroutine _fadeCoroutine;
    private float _startSpeed;
    private bool _canMove;

    private void OnEnable()
    {
        if (_componentsData.IsAi)
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
        
        if (!_componentsData.CanComponentsWork || _componentsData.IsDead) return;
		
        _componentsData.Animator.SetTrigger(AnimatorHash.IsFirstSkill);
        _componentsData.Animator.DOLayerWeight(4, 1f, BlendAttackLayerDuration);
        _componentsData.SetComponentsWorkState(false);
        
        var toRotation = Quaternion.LookRotation(_destinationPoint.position - _componentsData.RotationRoot.position, Vector3.up);
        _componentsData.RotationRoot.DORotate(toRotation.eulerAngles, 0.2f);

        StartCoroutine(OnActivate());
		
        IEnumerator OnActivate()
        {
            var currentTime = 0.0f;
            var wait = new WaitForEndOfFrame();
            var targetDirection = _destinationPoint.position - transform.parent.position;
            _canMove = true;
            _collider.enabled = true;

            while (currentTime < _timeMove)
            {
                if (_canMove)
                    _componentsData.CharacterController.Move(targetDirection.normalized * _speedMove * Time.deltaTime);
                
                currentTime += Time.deltaTime;
                
                yield return wait;
            }

            _collider.enabled = false;
            
            var skillDamage = Instantiate(_damagePrefab, _skillSpawnPoint);
            skillDamage.Init(_componentsData);
            skillDamage.gameObject.SetActive(true);
            skillDamage.gameObject.transform.SetParent(null);
			
            yield return new WaitForSeconds(2);
            
            _componentsData.Animator.DOLayerWeight(4, 0f, BlendAttackLayerDuration);
            _componentsData.SetComponentsWorkState(true);
        }
    }
    
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == 3 
            || collider == _excludeCollider 
            || collider.gameObject.layer == 13) return;
        
        _canMove = false;
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