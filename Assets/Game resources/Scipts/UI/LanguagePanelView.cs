using System.Collections.Generic;
using UnityEngine;

public class LanguagePanelView : MonoBehaviour
{
    [SerializeField] private List<LanguageViewCard> _languageViewCards;
    [SerializeField] private LanguageViewCard _currentLanguageViewCard;

    public void SetLanguage(LanguageViewCard languageViewCard)
    {
        _currentLanguageViewCard.Unselect();
        languageViewCard.Select();
        _currentLanguageViewCard = languageViewCard;
    }
}
