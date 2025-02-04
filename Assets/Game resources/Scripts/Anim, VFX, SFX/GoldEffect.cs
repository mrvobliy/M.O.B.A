using DG.Tweening;
using TMPro;
using UnityEngine;

public class GoldEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private SpriteRenderer _coinImg;
    [SerializeField] private Vector3 _effectPosOffset;
    
    [Header("TEXT SETTINGS")]
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Transform _textMoveRoot;
    [SerializeField] private Transform _textLookRoot;
    [SerializeField] private float _timeTextAnim;
    [SerializeField] private Vector3 _posTextOffset;
    [SerializeField] private Vector3 _endMinOffset;
    [SerializeField] private Vector3 _endMaxOffset;

    public void Play(Transform point, int goldValue)
    {
        _effect.Play();
        transform.position = point.position + _effectPosOffset;
        _text.text = goldValue + "";
        PlayTextAnimation();
        
        Invoke(nameof(DelayedDisable), 1);
    }

    private void PlayTextAnimation()
    {
        _text.alpha = 1;
        _coinImg.color = Color.white;
        _textMoveRoot.localPosition = _posTextOffset;
        _textMoveRoot.localScale = new Vector3(0, 0, 0);
        
        var posX = Random.Range(_endMinOffset.x, _endMaxOffset.x);
        var posY = Random.Range(_endMinOffset.y, _endMaxOffset.y);
        var posZ = Random.Range(_endMinOffset.z, _endMaxOffset.z);
        var endPos = new Vector3(posX, posY, posZ);
        
        _textLookRoot.LookAt(Camera.main.transform);
        _textMoveRoot.DOScale(0.5f, _timeTextAnim).SetEase(Ease.OutBack);
        _textMoveRoot.DOLocalMove(_textMoveRoot.localPosition + endPos, _timeTextAnim).SetEase(Ease.OutBack);
        
        _text.DOFade(0, _timeTextAnim).SetEase(Ease.InExpo);
        _coinImg.DOFade(0, _timeTextAnim).SetEase(Ease.InExpo);
    }

    private void DelayedDisable() => gameObject.SetActive(false);
}