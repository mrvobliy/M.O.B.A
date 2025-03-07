using DG.Tweening;
using UnityEngine;

public class TowerAttackRadius : MonoBehaviour
{
    [SerializeField] private EntityComponentsData _componentsData;
    [SerializeField] private ParticleSystem _radius;
    [SerializeField] private float _timeAnim;
    [SerializeField] private float _delayToHide;

    private Renderer _renderer;

    private void OnEnable()
    {
        _componentsData.EntityAttackControl.OnPlayerFound += Show;
        _componentsData.OnDeathStart += Hide;
        _renderer = _radius.GetComponent<Renderer>();
    }

    private void OnDisable()
    {
        _componentsData.EntityAttackControl.OnPlayerFound -= Show;
        _componentsData.OnDeathStart -= Hide;
    }

    private void Show()
    {
        CancelInvoke(nameof(Hide));
        DelayInvokeHide();
        
       if (_radius.gameObject.activeSelf) return;
       
        _radius.gameObject.SetActive(true);
        _renderer.material.DOFade(1, _timeAnim);
    }

    private void DelayInvokeHide()
    {
        CancelInvoke(nameof(Hide));
        Invoke(nameof(Hide), _delayToHide);
    }

    private void Hide()
    {
        _renderer.material.DOFade(0, _timeAnim).OnComplete(() =>
        {
            _radius.gameObject.SetActive(false);
        });
    }
}