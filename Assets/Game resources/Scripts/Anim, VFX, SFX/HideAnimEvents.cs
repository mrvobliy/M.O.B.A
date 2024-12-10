using System;
using UnityEngine;

public class HideAnimEvents : MonoBehaviour
{
    public Action AnimationHideEvent;

    public void AnimationHide()
    {
        AnimationHideEvent?.Invoke();
    }
}
