using UnityEngine;

public class HeroRespawnControl : MonoBehaviour
{
    [SerializeField] private EntityComponentsData _entityComponentsData;
    [SerializeField] private HeroHealthControl _heroHealthControl;

    private void OnEnable() => _entityComponentsData.EntityHealthControl.OnDeathEnd += Respawn;

    private void OnDisable() => _entityComponentsData.EntityHealthControl.OnDeathEnd -= Respawn;

    private void Respawn()
    {
        _entityComponentsData.EntityHealthControl.Animator.SetLayerWeight(6, 0);
        _entityComponentsData.EntityHealthControl.Animator.SetBool(AnimatorHash.IsDeath, false);
        var team = _entityComponentsData.EntityTeam;
        transform.parent.position = HeroSpawnControl.Instance.GetPoint(team).position;
        _entityComponentsData.EntityHealthControl.RotationParent.rotation = HeroSpawnControl.Instance.GetPoint(team).rotation;
        _heroHealthControl.Agent.enabled = true;
        _heroHealthControl.RestoreFullHeath();
        
        HeroSpawnControl.Instance.PlayEffect(team);
    }
}