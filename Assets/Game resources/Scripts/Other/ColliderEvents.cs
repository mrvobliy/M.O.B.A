using System;
using UnityEngine;

public class ColliderEvents : MonoBehaviour
{
    public Action<Collider> ColliderEnter; 
    public Action<Collider> ColliderExit;

    private bool _isEnable;

    public void SetWorkState(bool isEnable) => _isEnable = isEnable;
    
    private void OnTriggerEnter(Collider collider)
    {
        if (!_isEnable) return;
        
        ColliderEnter?.Invoke(collider);
    }

    private void OnTriggerStay(Collider collider)
    {
        if (!_isEnable) return;
        
        ColliderEnter?.Invoke(collider);
    }

    private void OnTriggerExit(Collider collider)
    {
        if (!_isEnable) return;
        
        ColliderExit?.Invoke(collider);
    }
}