using System;
using TMPro;
using UnityEngine;

public class ChangeDebugVariable : MonoBehaviour
{
    [SerializeField] private IntVariable _intVariable;
    [SerializeField] private string _paramName;
    [SerializeField] private TMP_Text _paramNameText;
    [SerializeField] private TMP_Text _currentValueText;
    [SerializeField] private TMP_InputField _inputField;

    private void OnEnable()
    {
        if (_inputField != null)
        {
            _inputField.onEndEdit.AddListener(ValidateInput);
        }

        _paramNameText.text = _paramName;
        _currentValueText.text = _intVariable.Value + "";
    }

    private void OnDisable()
    {
        if (_inputField != null)
        {
            _inputField.onEndEdit.RemoveListener(ValidateInput);
        }
    }
    
    private void ValidateInput(string input)
    {
        if (int.TryParse(input, out int intValue))
        {
            _intVariable.Set(intValue);
            _currentValueText.text = _intVariable.Value + "";
        }
        else
        {
            _inputField.text = string.Empty;
        }
    }
}
