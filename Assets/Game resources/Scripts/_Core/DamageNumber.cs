using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Vector3 _startOffset;
    [SerializeField] private Vector3 _endMinOffset;
    [SerializeField] private Vector3 _endMaxOffset;
    [SerializeField] private float _timeAnim;

    private void OnEnable()
    {
        transform.LookAt(Camera.main.transform);

        transform.localScale = new Vector3(0, 0, 0);

        var posX = Random.Range(_endMinOffset.x, _endMaxOffset.x);
        var posY = Random.Range(_endMinOffset.y, _endMaxOffset.y);
        var posZ = Random.Range(_endMinOffset.z, _endMaxOffset.z);

        var endPos = new Vector3(posX, posY, posZ);

        transform.DOScale(1, _timeAnim).SetEase(Ease.OutBack);
        transform.DOMove(transform.position + endPos, _timeAnim).SetEase(Ease.OutBack);
        _text.DOFade(0, _timeAnim).SetEase(Ease.InExpo).OnComplete(() => {Destroy(gameObject);});
    }

    public void SetDamageText(int damage)
    {
        _text.text = damage + "";
    }
}
