using DG.Tweening;
using UnityEngine;

public class UiPanelAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private float _startPos;
    [SerializeField] private float _timeAnim;

    public void Show()
    {
        _rectTransform.gameObject.SetActive(true);
        _rectTransform.DOLocalMoveY(0, _timeAnim);
    }

    public void Hide()
    {
        _rectTransform.DOLocalMoveY(_startPos, _timeAnim).OnComplete(() =>
        {
            _rectTransform.gameObject.SetActive(false);
        });
    }
}
