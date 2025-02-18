using UnityEngine;

public class HeroRespawnControl : MonoBehaviour
{
    [SerializeField] private EntityComponentsData _componentsData;
    [SerializeField] private HeroHealthControl _heroHealthControl;

    private void OnEnable() => _componentsData.EntityHealthControl.OnDeathEnd += Respawn;
    private void OnDisable() => _componentsData.EntityHealthControl.OnDeathEnd -= Respawn;

    private void Respawn()
    {
        _componentsData.Animator.SetLayerWeight(6, 0);
        _componentsData.Animator.SetBool(AnimatorHash.IsDeath, false);
        
        var team = _componentsData.EntityTeam;
        transform.parent.position = HeroSpawnManger.Instance.GetPoint(team).position;
        _componentsData.RotationRoot.rotation = HeroSpawnManger.Instance.GetPoint(team).rotation;
        
        _componentsData.NavMeshAgent.enabled = true;
        _componentsData.Collider.enabled = true;
        _heroHealthControl.RestoreFullHeath();
        
        HeroSpawnManger.Instance.PlayEffect(team);
    }
}