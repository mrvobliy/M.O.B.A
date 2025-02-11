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
    [SerializeField] private List<RewardModel> _rewards;
    [SerializeField] private OllieChestRewardView _rewardView;
    [SerializeField] private Transform _rewardViewRoot;
    [Space] 
    [SerializeField] private Animator _animator;
    [Space] 
    [SerializeField] private ParticleSystem _getRewardEffect;
    [SerializeField] private ParticleSystem _openChestEffect;
    [Space] 
    [SerializeField] private float _delayBetweenGetReward;
    
    private readonly List<OllieChestRewardView> _rewardsViews = new();

    private void OnEnable() => _openChestButton.onClick.AddListener(OpenChest);
    private void OnDisable() => _openChestButton.onClick.RemoveListener(OpenChest);

    [Button]
    public void ShowChest()
    {
        _animator.SetBool("Hide", false);
        _animator.SetTrigger("Show");
    }

    [Button]
    public void OpenChest()
    {
        var seq = DOTween.Sequence();

        seq.AppendCallback(() => {_animator.SetTrigger("OpenChest");});
        seq.AppendInterval(1);
        seq.AppendCallback(ShowRewardViews);
    }

    public void PlayEffect() => _getRewardEffect.Play();
    public void PlayOpenChestEffect() => _openChestEffect.Play();

    private void ShowRewardViews()
    {
        StartCoroutine(OnStartMove());
        return;
            
        IEnumerator OnStartMove()
        {
            var waitTime = new WaitForSeconds(_delayBetweenGetReward);
            
            foreach (var reward in _rewards)
            {
                _rewardView.Init(reward);
                _animator.SetTrigger("Get");
                
                yield return waitTime;
            }
            
            _animator.SetBool("Hide", true);
            DestroyViews();
        }
    }

    private void DestroyViews()
    {
        foreach (var rewardView in _rewardsViews)
        {
            Destroy(rewardView.gameObject);
        }
    }
}