using System.Collections;
using System.Collections.Generic;
using AssetKits.ParticleImage;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OllieChestAnimation : MonoBehaviour
{
    [SerializeField] private Transform _closeChestIcon;
    [SerializeField] private Transform _chestCover;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _shine;
    [SerializeField] private TMP_Text _openChestButtonText;
    [SerializeField] private Button _openChestButton;
    [Space]
    [SerializeField] private RewardModel[] _rewards;
    [SerializeField] private OllieChestRewardView _rewardViewPrefab;
    [SerializeField] private Transform _rewardViewRoot;
    [Space] 
    [SerializeField] private Transform _rewardViewEndPoint;
    [SerializeField] private float _timeShowRewardView;
    [Space] 
    [SerializeField] private ParticleImage _openEffect;
    [SerializeField] private ParticleImage _lightLineEffect;
    [SerializeField] private Animator _animator;
    
    private readonly List<OllieChestRewardView> _rewardsViews = new();

    private void OnEnable() => _openChestButton.onClick.AddListener(OpenChest);
    private void OnDisable() => _openChestButton.onClick.RemoveListener(OpenChest);

    [Button]
    public void ShowChest()
    {
        /*var seq = DOTween.Sequence();
        seq.Append(_canvasGroup.DOFade(1, 0.35f));
        seq.AppendCallback(() => {_closeChestIcon.gameObject.SetActive(true);});
        seq.Append(_closeChestIcon.DOScale(0.75f, 0.25f));
        seq.Join(_shine.transform.DOScale(1, 0.25f));
        seq.Join(_openChestButtonText.DOFade(0.16f, 0.3f));
        seq.JoinCallback(() => {_openChestButton.gameObject.SetActive(true);});
        seq.Append(_closeChestIcon.DOScale(0.7f, 0.15f));*/

        _animator.SetTrigger("Show");
        SpawnViews();
    }

    [Button]
    public void OpenChest()
    {
        var seq = DOTween.Sequence();
        /*seq.Append(_closeChestIcon.DOLocalMove(new Vector3(0, -900, 0), 0.3f));
        
        seq.Join(_openChestButtonText.DOFade(0, 0.2f));
        seq.JoinCallback(() => {_openChestButton.gameObject.SetActive(false);});
        
        seq.Join(_closeChestIcon.DOScale(new Vector3(0.7f, 0.45f, 0.7f), 0.3f));
        seq.Append(_closeChestIcon.DOScale(new Vector3(0.7f, 0.8f, 0.7f), 0.3f));
        
        seq.Append(_chestCover.DOMove(new Vector3(0, 5100, 0), 0.7f));
        seq.InsertCallback(0.7f, () => {_chestCover.gameObject.SetActive(false);});
        seq.Join(_closeChestIcon.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.3f));
        
        seq.JoinCallback(() => { _lightLineEffect.Play();});
        seq.JoinCallback(() => {_openEffect.Play();});*/

        seq.AppendCallback(() => {_animator.SetTrigger("OpenChest");});
        seq.AppendInterval(1);
        seq.AppendCallback(() =>
        {
            StartCoroutine(OnStartMove());
            return;
            
            IEnumerator OnStartMove()
            {
                var waitTime = new WaitForSeconds(0.7f);
                
                for (var i = 0; i < _rewardsViews.Count; i++)
                {
                    var rewardView = _rewardsViews[i];
                    
                    var seqRewardsViews = DOTween.Sequence();
                    seqRewardsViews.AppendCallback(() => {rewardView.transform.SetParent(_rewardViewEndPoint);});
                    seqRewardsViews.Append(rewardView.transform.DOLocalMove(Vector3.zero, 0.5f));
                    seqRewardsViews.Join(rewardView.transform.DOScale(1.5f, 0.5f));
                    seqRewardsViews.Append(rewardView.transform.DOScale(1.3f, 0.2f));
                    
                    seqRewardsViews.Append(rewardView.Text.DOFade(1, 0.5f));
                    
                    /*_closeChestIcon.DOScale(0.65f, 0.2f).OnComplete(() => { 
                        _closeChestIcon.DOScale(0.7f, 0.2f);});*/
                    
                    _animator.SetTrigger("Get");
                    
                    yield return waitTime;
                    
                    _rewardsViews[i].transform.DOScale(0, 0.3f);
                }

                _animator.enabled = false;
                var seq = DOTween.Sequence();
                seq.Append(_closeChestIcon.DOScale(0.73f, 0.2f));
                seq.Append(_closeChestIcon.DOScale(0, 0.2f));
                seq.Append(_canvasGroup.DOFade(0, 0.4f));
            }
        });
    }

    private void SpawnViews()
    {
        foreach (var reward in _rewards)
        {
            var view = Instantiate(_rewardViewPrefab, _rewardViewRoot);
            view.Init(reward);
            _rewardsViews.Add(view);
        }
    }
}