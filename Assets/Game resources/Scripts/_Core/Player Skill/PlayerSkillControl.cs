using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerSkillControl : MonoBehaviour
{
    [SerializeField] private RectTransform _skillButton;
    [SerializeField] private ButtonEvents _skillButtonEvents;
    [SerializeField] private PlayerHero _playerHero;
    [SerializeField] private DecalProjector _indicator;
    [SerializeField] private float _sensitivity = 0.1f;
    [SerializeField] private float _fadeSpeed = 2.0f;
    
    private Vector3 _previousToMouseDir;
    private Coroutine _fadeCoroutine;

    private void OnEnable()
    {
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
        _playerHero.ActivateSkill();
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
