using Sirenix.OdinInspector;
using UnityEngine;

public class EntityComponentsData : MonoBehaviour
{
    [SerializeField] private Team _entityTeam;
    [SerializeField] private EntityType _entityType;
    private bool _isHero => _entityType == EntityType.Hero;

    [SerializeField] private EntityHealthControl _entityHealthControl;
    
    [SerializeField, ShowIf(nameof(_isHero))] private HeroGoldControl _heroGoldControl;
    [SerializeField, ShowIf(nameof(_isHero))] private HeroExperienceControl _heroExperienceControl;

    public Team EntityTeam => _entityTeam;
    public EntityType EntityType => _entityType;
    public HeroGoldControl HeroGoldControl => _heroGoldControl;
    public HeroExperienceControl HeroExperienceControl => _heroExperienceControl;
    public EntityHealthControl EntityHealthControl => _entityHealthControl;
}
