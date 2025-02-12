using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private OllieChestAnimatorEvents _chestAnimatorEvents;
    [Space] 
    [SerializeField] private ParticleSystem _getRewardEffect;
    [SerializeField] private ParticleSystem _openChestEffect;
    [Space] 
    [SerializeField] private float _delayBetweenGetReward;
    
    private readonly List<OllieChestRewardView> _rewardsViews = new();

    private void OnEnable()
    {
        _openChestButton.onClick.AddListener(OpenChest);
        _chestAnimatorEvents.OnPlayGetRewardEffect += PlayGetRewardEffect;
        _chestAnimatorEvents.OpenChestEffect += PlayOpenChestEffect;
    }

    private void OnDisable()
    {
        _openChestButton.onClick.RemoveListener(OpenChest);
        _chestAnimatorEvents.OnPlayGetRewardEffect += PlayGetRewardEffect;
        _chestAnimatorEvents.OpenChestEffect += PlayOpenChestEffect;
    }
    
    [ContextMenu("ShowChest")]
    private void ShowChest()
    {
        _animator.SetBool("Hide", false);
        _animator.SetTrigger("Show");
    }

    [ContextMenu("OpenChest")]
    private void OpenChest()
    {
        _animator.SetTrigger("OpenChest");
        Invoke(nameof(ShowRewardViews), 1);
    }

    private void PlayGetRewardEffect() => _getRewardEffect.Play();
    private void PlayOpenChestEffect() => _openChestEffect.Play();

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