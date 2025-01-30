using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class PlayerGoldBalanceView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    
    private HeroGoldControl _heroGoldControl;
    
    public static PlayerGoldBalanceView Instance;

    private void Awake() => Instance = this;

    private void OnDisable()
    {
        if (_heroGoldControl != null)
            _heroGoldControl.OnBalanceChanged -= UpdateView;
    }

    public void SetHeroGoldControl(HeroGoldControl heroGoldControl)
    {
        _heroGoldControl = heroGoldControl;
        _heroGoldControl.OnBalanceChanged += UpdateView;
    }

    private void UpdateView(int balance)
    {
        _text.text = balance + "";
        _text.transform.DOScale(1.1f, 0.1f).OnComplete(() 
            => _text.transform.DOScale(1f, 0.1f));
    }
}