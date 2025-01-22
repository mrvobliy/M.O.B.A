using Sirenix.OdinInspector;
using UnityEngine;

public class EntityComponentsData : MonoBehaviour
{
    [SerializeField] private bool _isAi;
    [SerializeField] private Team _entityTeam;
    [SerializeField] private EntityType _entityType;
    [SerializeField] private EntityHealthControl _entityHealthControl;
    [SerializeField] private EntityMoveControl _entityMoveControl;
    
    private bool IsNeedShowAttackControl => _entityType != EntityType.Throne;
    [SerializeField, ShowIf(nameof(IsNeedShowAttackControl))] private EntityAttackControl _entityAttackControl;
    
    private bool IsNeedShowGoldExpoParam => _entityType == EntityType.Hero;
    [SerializeField, ShowIf(nameof(IsNeedShowGoldExpoParam))] private HeroGoldControl _heroGoldControl;
    [SerializeField, ShowIf(nameof(IsNeedShowGoldExpoParam))] private HeroExperienceControl _heroExperienceControl;
    
    public Team EntityTeam => _entityTeam;
    public EntityType EntityType => _entityType;
    public HeroGoldControl HeroGoldControl => _heroGoldControl;
    public HeroExperienceControl HeroExperienceControl => _heroExperienceControl;
    public EntityHealthControl EntityHealthControl => _entityHealthControl;
    public EntityMoveControl EntityMoveControl => _entityMoveControl;
    public EntityAttackControl EntityAttackControl => _entityAttackControl;
    public bool CanComponentsWork { get; private set; } = true;
    public bool IsAi => _isAi;

    public void SetWorkState(bool isCanWork) => CanComponentsWork = isCanWork;

    private void OnValidate()
    {
        TrySetEntityAttackControl();
        TrySetEntityMoveControl();
    }
    
    private void TrySetEntityMoveControl()
    {
        if (_entityMoveControl != null) 
            return;
        
        var component = transform.parent.GetComponentInChildren<EntityMoveControl>();
        
        if (component == null) 
            return;

        _entityMoveControl = component;
    }

    private void TrySetEntityAttackControl()
    {
        if (_entityAttackControl != null || !IsNeedShowAttackControl) 
            return;
        
        var component = transform.parent.GetComponentInChildren<EntityAttackControl>();
        
        if (component == null) 
            return;

        _entityAttackControl = component;
    }
}
