using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroSkillView : MonoBehaviour
{
    [SerializeField] private GameObject _skillEnableGroup;
    [SerializeField] private GameObject _skillDisableGroup;
    [SerializeField] private Image _manaSlider1;
    [SerializeField] private Image _manaSlider2;
    [SerializeField] private TMP_Text _timerText;

    [Button]
    public void PlayCooldownAnim(int time)
    {
        _skillEnableGroup.transform.DOScale(0, 0.15f);
        _skillDisableGroup.transform.DOScale(0.9f, 0.15f);
        
        _timerText.DOFade(1, 0.2f);
        
        StartCoroutine(OnCooldownAnim());
        return;

        IEnumerator OnCooldownAnim()
        {
            var waitTime = new WaitForSeconds(1);
            var currentTime = time;

            while (currentTime > 0)
            {
                _timerText.text = currentTime + "";
                currentTime--;
                yield return waitTime;
            }
            
            _skillEnableGroup.transform.DOScale(1, 0.15f);
            _skillDisableGroup.transform.DOScale(0, 0.15f);
            
            _timerText.DOFade(0, 0.2f);
        }
    }

    public void SetManaSlider()
    {
        
    }
}