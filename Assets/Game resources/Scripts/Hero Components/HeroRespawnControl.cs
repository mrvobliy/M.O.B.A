using Sirenix.OdinInspector;
using UnityEngine;

public class HeroRespawnControl : MonoBehaviour
{
    [SerializeField] private EntityComponentsData _componentsData;
    [SerializeField] private HeroHealthControl _heroHealthControl;

    private void OnEnable() => _componentsData.OnDeathEnd += Respawn;
    private void OnDisable() => _componentsData.OnDeathEnd -= Respawn;

    [Button]
    private void Respawn()
    {
        _componentsData.Animator.SetLayerWeight(6, 0);
        _componentsData.Animator.SetBool(AnimatorHash.IsDeath, false);
        
        var team = _componentsData.EntityTeam;
        
        var respawnPoint = HeroSpawnManger.Instance.GetPoint(team).position;
        transform.parent.position = respawnPoint;
        
        _componentsData.RotationRoot.localPosition = Vector3.zero;
        _componentsData.RotationRoot.localRotation = HeroSpawnManger.Instance.GetPoint(team).rotation;
        
        _heroHealthControl.RestoreFullHeath();
        
        HeroSpawnManger.Instance.PlayEffect(team);
        
        Invoke(nameof(EnableComponents), 0.1f);
    }

    private void EnableComponents() => _componentsData.SetComponentsWorkState(true);
}