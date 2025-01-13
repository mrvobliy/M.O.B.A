using Sirenix.OdinInspector;
using UnityEngine;

public class EntityComponentsData : MonoBehaviour
{
    [SerializeField] private Team _entityTeam;
    [SerializeField] private EntityType _entityType;
    [SerializeField] private EntityHealthControl _entityHealthControl;
    
    private bool IsNeedShowAttackControl => _entityType != EntityType.Trone;
    [SerializeField, ShowIf(nameof(IsNeedShowAttackControl))] private EntityAttackControl _entityAttackControl;
    
    private bool IsNeedShowGoldExpoParam => _entityType == EntityType.Hero;
    [SerializeField, ShowIf(nameof(IsNeedShowGoldExpoParam))] private HeroGoldControl _heroGoldControl;
    [SerializeField, ShowIf(nameof(IsNeedShowGoldExpoParam))] private HeroExperienceControl _heroExperienceControl;

    public Team EntityTeam => _entityTeam;
    public EntityType EntityType => _entityType;
    public HeroGoldControl HeroGoldControl => _heroGoldControl;
    public HeroExperienceControl HeroExperienceControl => _heroExperienceControl;
    public EntityHealthControl EntityHealthControl => _entityHealthControl;
    public EntityAttackControl EntityAttackControl => _entityAttackControl;
    public bool CanComponentsWork { get; private set; }

    public void SetWorkState(bool isCanWork) => CanComponentsWork = isCanWork;

    private void OnValidate()
    {
        if (_entityAttackControl != null || !IsNeedShowAttackControl) 
            return;
        
        var entityAttackControl = transform.parent.GetComponentInChildren<EntityAttackControl>();
        
        if (entityAttackControl == null) 
            return;

        _entityAttackControl = entityAttackControl;
    } 
}
