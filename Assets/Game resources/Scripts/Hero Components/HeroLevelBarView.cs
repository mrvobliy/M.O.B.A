using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroLevelBarView : MonoBehaviour
{
    [SerializeField] private HeroExperienceControl _experienceControl;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private Image _fillImage;
    [SerializeField] private Image _levelDoneImage;
    [SerializeField] private Transform _root;
    [SerializeField] private float _timeAnim;

    private void OnEnable()
    {
        _experienceControl.OnLevelChanged += UpdateLevelView;
        _experienceControl.OnExperienceChanged += UpdateExperienceView;
    }

    private void OnDisable()
    {
        _experienceControl.OnLevelChanged -= UpdateLevelView;
        _experienceControl.OnExperienceChanged -= UpdateExperienceView;
    }

    private void UpdateLevelView(int level)
    {
        _levelText.text = level + "";
        
        _root.DOScale(1.2f, _timeAnim).OnComplete(() =>
        {
            _root.DOScale(1f, _timeAnim);
        });
        
        _levelDoneImage.DOFade(1, _timeAnim).OnComplete(() =>
        {
            _levelDoneImage.DOFade(0, _timeAnim);
        });
    }

    private void UpdateExperienceView(float experienceAttitude) => _fillImage.fillAmount = experienceAttitude;
}