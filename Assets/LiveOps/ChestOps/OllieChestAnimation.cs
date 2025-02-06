using System;
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
    //[SerializeField] private Transform _openChestIcon;
    [SerializeField] private Transform _chestCover;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _shine;
    [SerializeField] private TMP_Text _openChestButtonText;
    [SerializeField] private TMP_Text _takeChestButtonText;
    [SerializeField] private Button _openChestButton;
    [SerializeField] private Button _takeChestButton;
    [Space]
    [SerializeField] private RewardModel[] _rewards;
    [SerializeField] private OllieChestRewardView _rewardViewPrefab;
    [SerializeField] private Transform _rewardViewRoot;
    [Space] 
    [SerializeField] private List<Transform> _rows;
    [SerializeField] private List<Transform> _points;
    [SerializeField] private float _timeShowRewardView;
    [Space] 
    [SerializeField] private ParticleImage _openEffect;
    [SerializeField] private ParticleImage _lightLineEffect;
    
    private List<OllieChestRewardView> _rewardsViews = new();

    private void OnEnable()
    {
        _openChestButton.onClick.AddListener(OpenChest);
        _takeChestButton.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        _openChestButton.onClick.RemoveListener(OpenChest);
        _takeChestButton.onClick.RemoveListener(Close);
    }

    [Button]
    public void ShowChest()
    {
        var seq = DOTween.Sequence();
        seq.Append(_canvasGroup.DOFade(1, 0.35f));
        seq.AppendCallback(() => {_closeChestIcon.gameObject.SetActive(true);});
        seq.Append(_closeChestIcon.DOScale(0.75f, 0.25f));
        seq.Join(_shine.transform.DOScale(1, 0.25f));
        seq.Join(_openChestButtonText.DOFade(0.16f, 0.3f));
        seq.JoinCallback(() => {_openChestButton.gameObject.SetActive(true);});
        seq.Append(_closeChestIcon.DOScale(0.7f, 0.15f));

        SpawnViews();
    }

    [Button]
    public void OpenChest()
    {
        var seq = DOTween.Sequence();
        seq.Append(_closeChestIcon.DOLocalMove(new Vector3(0, -900, 0), 0.3f));
        
        seq.Join(_openChestButtonText.DOFade(0, 0.2f));
        seq.JoinCallback(() => {_openChestButton.gameObject.SetActive(false);});
        
        seq.Join(_closeChestIcon.DOScale(new Vector3(0.7f, 0.45f, 0.7f), 0.3f));
        seq.Append(_closeChestIcon.DOScale(new Vector3(0.7f, 0.8f, 0.7f), 0.3f));
        seq.Append(_chestCover.DOMove(new Vector3(-16, 5100, 0), 0.7f));
        seq.Join(_closeChestIcon.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.3f));
        seq.JoinCallback(() => { _lightLineEffect.Play();});
        seq.JoinCallback(() => {_openEffect.Play();});
        seq.AppendCallback(() =>
        {
            var countRows = _rewardsViews.Count / 4;

            if (_rewardsViews.Count % 4 != 0)
                countRows++;

            for (var i = 0; i < countRows; i++) 
                _rows[i].gameObject.SetActive(true);
            
            StartCoroutine(OnStartMove());
            return;

            IEnumerator OnStartMove()
            {
                var waitTime = new WaitForSeconds(_timeShowRewardView);
                
                for (var i = 0; i < _rewardsViews.Count; i++) 
                    _points[i].gameObject.SetActive(true);
                
                for (var i = 0; i < _rewardsViews.Count; i++)
                {
                    var rewardView = _rewardsViews[i];
                    var point = _points[i];
                    
                    var seqRewardsViews = DOTween.Sequence();
                    seqRewardsViews.AppendCallback(() => {rewardView.transform.SetParent(point);});
                    seqRewardsViews.Append(rewardView.transform.DOLocalMove(Vector3.zero, _timeShowRewardView));
                    seqRewardsViews.Join(rewardView.transform.DOScale(1, _timeShowRewardView));
                    seqRewardsViews.Append(rewardView.Text.DOFade(1, _timeShowRewardView));
                    
                    _closeChestIcon.DOScale(0.65f, _timeShowRewardView / 2).OnComplete(() => { 
                        _closeChestIcon.DOScale(0.7f, _timeShowRewardView / 2);});
                    
                    yield return waitTime;
                }
                
                var seq = DOTween.Sequence();
                seq.Append(_takeChestButtonText.DOFade(0.16f, 0.3f));
                seq.AppendCallback(() => {_takeChestButton.gameObject.SetActive(true);});
                
                var seq2 = DOTween.Sequence();
                seq2.Append(_closeChestIcon.DOScale(0.73f, 0.2f));
                seq2.Append(_closeChestIcon.DOScale(0, 0.2f));
            }
        });
    }

    private void Close()
    {
        _canvasGroup.DOFade(0, 0.35f);
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