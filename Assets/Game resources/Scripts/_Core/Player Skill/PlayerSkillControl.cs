using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerSkillControl : MonoBehaviour
{
    [SerializeField] private RectTransform _skillButton;
    [SerializeField] private ButtonEvents _skillButtonEvents;
    [SerializeField] private PlayerHero _playerHero;
    [SerializeField] private DecalProjector _indicator;
    [SerializeField] private float _sensitivity = 0.1f;
    
    private Vector3 _previousToMouseDir;
    
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
        _indicator.fadeFactor = 0.3f;
        var toMouseDir = Input.mousePosition - _skillButton.position;
        _previousToMouseDir = toMouseDir;
    }
    
    private void ReleaseSkill()
    {
        _indicator.fadeFactor = 0.0f;
        _playerHero.ActivateSkill();
    }

    private void RotateIndicator()
    {
        var toMouseDir = Input.mousePosition - _skillButton.position;
        var anglePrevious = Mathf.Atan2(_previousToMouseDir.y, _previousToMouseDir.x) * Mathf.Rad2Deg;
        var angleCurrent = Mathf.Atan2(toMouseDir.y, toMouseDir.x) * Mathf.Rad2Deg;
        var angleDelta = Mathf.DeltaAngle(anglePrevious, angleCurrent);

        _indicator.transform.parent.Rotate(Vector3.forward, angleDelta * _sensitivity);

        _previousToMouseDir = toMouseDir;
    }
}
