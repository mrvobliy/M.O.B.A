using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguageViewCard : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject _outline;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _selectColor;
    [SerializeField] private LanguagePanelView _languagePanelView;
    [SerializeField] private Button _button;

    private void OnEnable()
    {
        _button.onClick.AddListener(Set);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Set);
    }

    private void Set()
    {
        _languagePanelView.SetLanguage(this);
    }

    public void Select()
    {
        _text.color = _selectColor;
        _outline.SetActive(true);
    }

    public void Unselect()
    {
        _text.color = _defaultColor;
        _outline.SetActive(false);
    }
}
