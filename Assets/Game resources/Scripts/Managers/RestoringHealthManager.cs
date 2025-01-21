using UnityEngine;

public class RestoringHealthManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.TryGetComponent(out HeroRestoreHealthControl heroRestoreHealthControl)) return;
        
        heroRestoreHealthControl.StartRestoring();
    }

    private void OnTriggerExit(Collider collider)
    {
        if (!collider.TryGetComponent(out HeroRestoreHealthControl heroRestoreHealthControl)) return;
        
        heroRestoreHealthControl.StopRestoring();
    } 
}