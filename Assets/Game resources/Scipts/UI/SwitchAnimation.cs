using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchAnimation : MonoBehaviour
{
    [SerializeField] private TMP_Text _leftText;
    [SerializeField] private TMP_Text _rightText;
    [SerializeField] private TMP_Text _textOnSwitch;
    [Space] 
    [SerializeField] private float _leftPos;
    [SerializeField] private float _rightPos;
    [SerializeField] private float _timeAnim;
    [SerializeField] private Button _button;

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
        }
        else
        {
            _isRightPos = transform;
        }
    }
}
