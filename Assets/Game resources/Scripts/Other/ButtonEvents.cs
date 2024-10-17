using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEvents : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnButtonDown;
    public event Action OnButtonUp;
    public event Action OnButtonDrag;

    public void OnDrag(PointerEventData eventData)
    {
        OnButtonDrag?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        OnButtonDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnButtonUp?.Invoke();
    }
}
