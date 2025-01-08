using UnityEngine;

public class HeroRespawnControl : MonoBehaviour
{
    [SerializeField] private EntityComponentsData _entityComponentsData;

    private void OnEnable() => _entityComponentsData.EntityHealthControl.OnDeathEnd += Respawn;

    private void OnDisable() => _entityComponentsData.EntityHealthControl.OnDeathEnd -= Respawn;

    private void Respawn()
    {
        _entityComponentsData.EntityHealthControl.Animator.SetLayerWeight(6, 0);
        _entityComponentsData.EntityHealthControl.Animator.SetBool(AnimatorHash.IsDeath, false);
        var team = _entityComponentsData.EntityTeam;
        transform.parent.position = HeroSpawnControl.Instance.GetPoint(team).position;
        _entityComponentsData.EntityHealthControl.RotationParent.rotation = HeroSpawnControl.Instance.GetPoint(team).rotation;
        _entityComponentsData.EntityHealthControl.Agent.enabled = true;
        _entityComponentsData.EntityHealthControl.RestoreFullHeath();
        
        HeroSpawnControl.Instance.PlayEffect(team);
    }
}