using DG.Tweening;
using UnityEngine;

public class ButtonAnimations : MonoBehaviour
{
    [SerializeField] private ButtonEvents _buttonEvents;
    [SerializeField] private Animator _animator;

    private void OnEnable()
    {
        _buttonEvents.OnButtonDown += PlayButtonDownAnim;
        _buttonEvents.OnButtonUp += PlayButtonUpAnim;
    }

    private void OnDisable()
    {
        _buttonEvents.OnButtonDown -= PlayButtonDownAnim;
        _buttonEvents.OnButtonUp -= PlayButtonUpAnim;
    }

    private void PlayButtonDownAnim() => transform.DOScale(0.8f, 0.1f);
    private void PlayButtonUpAnim() => transform.DOScale(1f, 0.1f);
}