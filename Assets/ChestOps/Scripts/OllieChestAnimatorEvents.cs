using System;
using UnityEngine;

public class OllieChestAnimatorEvents : MonoBehaviour
{
    public Action OnPlayGetRewardEffect;
    public Action OpenChestEffect;

    public void PlayGetRewardEffect() => OnPlayGetRewardEffect?.Invoke();
    public void PlayOpenChestEffect() => OpenChestEffect?.Invoke();
}