using UnityEngine;

public class RestoringHealthManager : MonoBehaviour
{
    [SerializeField] private Team _team;
    
    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.TryGetComponent(out HeroRestoreHealthControl heroRestoreHealthControl)) return;
        
        if (heroRestoreHealthControl.ComponentsData.EntityTeam != _team) return;
        
        heroRestoreHealthControl.StartRestoring();
    }

    private void OnTriggerExit(Collider collider)
    {
        if (!collider.TryGetComponent(out HeroRestoreHealthControl heroRestoreHealthControl)) return;
        
        if (heroRestoreHealthControl.ComponentsData.EntityTeam != _team) return;
        
        heroRestoreHealthControl.StopRestoring();
    } 
}