using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SwordGirlThirdSkillControl : MonoBehaviour
{
    [SerializeField] private RectTransform _skillButton;
    [SerializeField] private ButtonEvents _skillButtonEvents;
    [SerializeField] private PlayerHero _playerHero;
    [SerializeField] private DecalProjector _indicator;
    [SerializeField] private float _sensitivity = 0.1f;
    [SerializeField] private float _fadeSpeed = 2.0f;
    [SerializeField] private PlayerSkillDamage _skillDamagePrefab;
    
    private Coroutine _fadeCoroutine;
    private Vector3 _previousToMousePos;
    
    private void OnEnable()
    {
        _skillButtonEvents.OnButtonDown += StartSkill;
        _skillButtonEvents.OnButtonUp += ReleaseSkill;
        _skillButtonEvents.OnButtonDrag += MoveIndicator;
    }

    private void OnDisable()
    {
        _skillButtonEvents.OnButtonDown -= StartSkill;
        _skillButtonEvents.OnButtonUp -= ReleaseSkill;
        _skillButtonEvents.OnButtonDrag -= MoveIndicator;
    }

    private void StartSkill()
    {
        StartFade(0.3f);
        var startPos = _indicator.transform.localPosition;
        startPos.x = 0;
        startPos.z = 0;
        _indicator.transform.localPosition = startPos;
        
        MoveIndicator();
    }
    
    private void ReleaseSkill()
    {
        StartFade(0.0f);
    }

    private void MoveIndicator()
    {
        var toMouseDir = Input.mousePosition - _previousToMousePos;
        var normDir = toMouseDir.normalized;
        var newPos = _indicator.transform.position + new Vector3(normDir.x, 0, normDir.y) * _sensitivity;
        newPos.y = _indicator.transform.position.y;
        _indicator.transform.position = newPos;
        
        _previousToMousePos = Input.mousePosition;
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
