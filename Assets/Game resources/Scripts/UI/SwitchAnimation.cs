using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchAnimation : MonoBehaviour
{
    [SerializeField] private TMP_Text _textOnSwitch;
    [Space] 
    [SerializeField] private float _leftPos;
    [SerializeField] private float _rightPos;
    [SerializeField] private float _timeAnim;
    [SerializeField] private Button _button;
    [SerializeField] private RectTransform _rectTransform;

    private bool _isRightPos;
    
    private void OnEnable()
    {
        _button.onClick.AddListener(Switch);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Switch);
    }

    private void Switch()
    {
        if (_isRightPos)
        {
            _isRightPos = false;
            _rectTransform.DOLocalMoveX(_leftPos, _timeAnim);
            _textOnSwitch.text = "on";
        }
        else
        {
            _isRightPos = true;
            _rectTransform.DOLocalMoveX(_rightPos, _timeAnim);
            _textOnSwitch.text = "off";
        }
    }
}
