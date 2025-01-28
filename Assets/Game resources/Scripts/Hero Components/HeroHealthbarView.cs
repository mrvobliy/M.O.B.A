using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HeroHealthbarView : MonoBehaviour
{
    [SerializeField] private EntityComponentsData _componentsData;
    [SerializeField] private Image _healthImage;
    [SerializeField] private Sprite _lightSideHealthImage;
    [SerializeField] private Sprite _darkSideHealthImage;
    [SerializeField] private RectTransform _barLine;
    [SerializeField] private Transform _barRoot;
    [SerializeField] private Transform _scaleRoot;
    [SerializeField] private CanvasGroup _whiteFront;
    [SerializeField] private float _timeAnim;
    [SerializeField] private float _scaleValue;
    [SerializeField] private bool _isPlayer;

    private const float StartLinePos = 0.52f;

    private void OnEnable()
    {
        _componentsData.EntityHealthControl.OnHealthChanged += UpdateBar;
        SetHealthLineColor();
    }

    private void OnDisable() => _componentsData.EntityHealthControl.OnHealthChanged -= UpdateBar;

    private void Update() => _barRoot.transform.LookAt(Camera.main.transform);

    private void UpdateBar()
    {
        var newLinePos = _barLine.localPosition;
        newLinePos.x = StartLinePos * (_componentsData.EntityHealthControl.HealthPercent - 1);
        _barLine.localPosition = newLinePos;

        PlayAnim();
    }

    private void PlayAnim()
    {
        _scaleRoot.DOScale(_scaleValue, _timeAnim).OnComplete(() =>
        {
            _scaleRoot.DOScale(1f, _timeAnim);
        });
        
        _whiteFront.DOFade(1, _timeAnim).OnComplete(() =>
        {
            _whiteFront.DOFade(0, _timeAnim);
        });
    }

    private void SetHealthLineColor()
    {
        if (_isPlayer) return;
        
        var playerTeam = VariableBase.PlayerTeam;
        var ourTeam = _componentsData.EntityTeam;
        
        _healthImage.sprite = playerTeam == ourTeam ? _lightSideHealthImage : _darkSideHealthImage;
    }
}