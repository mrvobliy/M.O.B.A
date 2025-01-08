using Sirenix.OdinInspector;
using UnityEngine;

public class EntityComponentsData : MonoBehaviour
{
    [SerializeField] private Team _entityTeam;
    [SerializeField] private EntityType _entityType;
    [SerializeField] private Target _target;
    private bool _isHero => _entityType == EntityType.Hero;

    [SerializeField] private EntityHealthControl _entityHealthControl;
    
    [SerializeField, ShowIf(nameof(_isHero))] private HeroGoldControl _heroGoldControl;
    [SerializeField, ShowIf(nameof(_isHero))] private HeroExperienceControl _heroExperienceControl;

    public Team EntityTeam => _entityTeam;
    public EntityType EntityType => _entityType;
    public Target Target => _target;
    public HeroGoldControl HeroGoldControl => _heroGoldControl;
    public HeroExperienceControl HeroExperienceControl => _heroExperienceControl;
    public EntityHealthControl EntityHealthControl => _entityHealthControl;

    private void OnEnable() => _target.OnDeath += SendEvent;

    private void OnDisable() => _target.OnDeath -= SendEvent;

    private void SendEvent()
    {
        _target.OnDeath -= SendEvent;
        EventsBase.OnEntityDeath(this);
    }
}
