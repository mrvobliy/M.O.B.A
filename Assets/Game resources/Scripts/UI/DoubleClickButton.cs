using UnityEngine;
using System;

public class DoubleClickButton : MonoBehaviour
{
    [SerializeField] private ButtonEvents _buttonEvents;
    public float doubleClickThreshold = 0.3f;

    private float lastClickTime;

    public event Action OnDoubleClick;

    private void OnEnable() => _buttonEvents.OnButtonUp += HandleClick;
    private void OnDisable() => _buttonEvents.OnButtonUp -= HandleClick;

    private void HandleClick()
    {
        var timeSinceLastClick = Time.time - lastClickTime;

        if (timeSinceLastClick <= doubleClickThreshold) 
            OnDoubleClick?.Invoke();

        lastClickTime = Time.time;
    }
}