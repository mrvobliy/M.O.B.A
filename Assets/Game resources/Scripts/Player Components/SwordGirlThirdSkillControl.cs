using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SwordGirlThirdSkillControl : MonoBehaviour
{
    [SerializeField] private PlayerHero _playerHero;
    [SerializeField] private EntityComponentsData _entityComponentsData;
    [SerializeField] private PlayerSkillDamage _skillDamagePrefab;
    [SerializeField] private RectTransform _skillButton;
    [SerializeField] private ButtonEvents _skillButtonEvents;
    [SerializeField] private DecalProjector _indicator;
    [SerializeField] private float _sensitivity = 0.1f;
    [SerializeField] private float _fadeSpeed = 2.0f;

    private Vector3 SpawnPoint => _indicator.transform.position + new Vector3(0, -1.22f, 0);
    
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
        _playerHero.ActivateThirdSkill(() =>
        {
            var skillDamage = Instantiate(_skillDamagePrefab, SpawnPoint, Quaternion.identity);
            skillDamage.Init(_entityComponentsData);
            skillDamage.gameObject.SetActive(true);
            skillDamage.gameObject.transform.SetParent(null);
        });
    }
    
    private void MoveIndicator()
    {
        var screenPos = Input.mousePosition;
        screenPos.z = Camera.main.WorldToScreenPoint(_indicator.transform.position).z;
        var worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        _indicator.transform.position = new Vector3(worldPos.x, _indicator.transform.position.y, worldPos.z + 0.5f);
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
