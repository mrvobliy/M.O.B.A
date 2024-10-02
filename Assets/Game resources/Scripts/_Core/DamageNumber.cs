using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Vector3 _startOffset;
    [SerializeField] private Vector3 _endMaxOffset;
    [SerializeField] private float _timeAnim;

    private void OnEnable()
    {
        transform.LookAt(Camera.main.transform);

        var startPos = transform.position;
        startPos += _startOffset;
        transform.position = startPos;

        var posX = Random.Range(0, _endMaxOffset.x);
        var posY = Random.Range(0, _endMaxOffset.y);
        var posZ = Random.Range(0, _endMaxOffset.z);

        var endPos = new Vector3(posX, posY, posZ);

        transform.DOMove(transform.position + endPos, _timeAnim).SetEase(Ease.OutBack);
        _text.DOFade(0, _timeAnim).OnComplete(() =>
        {
            Destroy(gameObject);
        }).SetEase(Ease.InExpo);
    }

    public void SetDamageText(int damage)
    {
        _text.text = damage + "";
    }
}
