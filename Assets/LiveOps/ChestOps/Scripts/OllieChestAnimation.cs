using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class OllieChestAnimation : MonoBehaviour
{
    [SerializeField] private Button _openChestButton;
    [Space]
    [SerializeField] private RewardModel[] _rewards;
    [SerializeField] private OllieChestRewardView _rewardViewPrefab;
    [SerializeField] private Transform _rewardViewRoot;
    [Space] 
    [SerializeField] private Animator _animator;
    
    private readonly List<OllieChestRewardView> _rewardsViews = new();

    private void OnEnable() => _openChestButton.onClick.AddListener(OpenChest);
    private void OnDisable() => _openChestButton.onClick.RemoveListener(OpenChest);

    [Button]
    public void ShowChest()
    {
        _animator.SetTrigger("Show");
        SpawnViews();
    }

    [Button]
    public void OpenChest()
    {
        var seq = DOTween.Sequence();

        seq.AppendCallback(() => {_animator.SetTrigger("OpenChest");});
        seq.AppendInterval(1);
        seq.AppendCallback(ShowRewardViews);
    }

    private void ShowRewardViews()
    {
        StartCoroutine(OnStartMove());
        return;
            
        IEnumerator OnStartMove()
        {
            var waitTime = new WaitForSeconds(0.5f);
                
            for (var i = 0; i < _rewardsViews.Count; i++)
            {
                _rewardsViews[i].gameObject.SetActive(true);
                _animator.SetTrigger("Get");
                    
                yield return waitTime;
                    
                _rewardsViews[i].gameObject.SetActive(false);
            }
            
            _animator.SetTrigger("Hide");
        }
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