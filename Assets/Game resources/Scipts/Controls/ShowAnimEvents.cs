using System;
using UnityEngine;

public class ShowAnimEvents : MonoBehaviour
{
    public Action AnimationShowEvent;

    public void AnimationShow()
    {
        AnimationShowEvent?.Invoke();
    }
}
