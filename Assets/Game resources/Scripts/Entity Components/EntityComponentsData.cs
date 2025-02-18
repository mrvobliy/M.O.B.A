using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class EntityComponentsData : MonoBehaviour
{
    [Title("Controls")]
    
    [SerializeField, ShowIf(nameof(IsNeedShowGoldExpoParam))] private bool _isAi;
    
    [SerializeField] private Team _entityTeam;
    [SerializeField] private EntityType _entityType;
    
    private bool IsNeedShowTowerTier => _entityType is EntityType.Tower;
    [SerializeField, ShowIf(nameof(IsNeedShowTowerTier))] private TowerTier _towerTier;
    
    [Title("Controls")]
    [SerializeField] private EntityHealthControl _entityHealthControl;
    
    private bool IsNeedShowEntityMoveControl => _entityType is EntityType.Tower;
    [SerializeField, HideIf(nameof(IsNeedShowEntityMoveControl))] private EntityMoveControl _entityMoveControl;
    
    private bool IsNeedShowAttackControl => _entityType != EntityType.Throne;
    [SerializeField, ShowIf(nameof(IsNeedShowAttackControl))] private EntityAttackControl _entityAttackControl;
    
    private bool IsNeedShowGoldExpoParam => _entityType == EntityType.Hero;
    [SerializeField, ShowIf(nameof(IsNeedShowGoldExpoParam))] private HeroGoldControl _heroGoldControl;
    [SerializeField, ShowIf(nameof(IsNeedShowGoldExpoParam))] private HeroExperienceControl _heroExperienceControl;
    
    private bool IsNeedShowCreepMoveControl => _entityType is EntityType.NeutralMelee or EntityType.NeutralRange;
    [SerializeField, ShowIf(nameof(IsNeedShowCreepMoveControl))] private CreepMoveControl _creepMoveControl;
    
    
    [Title("Components")]
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Collider _collider;
    [SerializeField] private Animator _animator;
    
    private bool IsNeedCharacterController => _entityType == EntityType.Hero;
    [SerializeField, ShowIf(nameof(IsNeedCharacterController))] private CharacterController _characterController;

    
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public Transform RotationRoot => _entityHealthControl.RotationParent;
    public Animator Animator => _animator;
    public Team EntityTeam => _entityTeam;
    public TowerTier TowerTier => _towerTier;
    public EntityType EntityType => _entityType;
    public CreepMoveControl CreepMoveControl => _creepMoveControl;
    public HeroGoldControl HeroGoldControl => _heroGoldControl;
    public HeroExperienceControl HeroExperienceControl => _heroExperienceControl;
    public EntityHealthControl EntityHealthControl => _entityHealthControl;
    public EntityMoveControl EntityMoveControl => _entityMoveControl;
    public EntityAttackControl EntityAttackControl => _entityAttackControl;
    public bool CanComponentsWork { get; private set; } = true;
    public bool IsAi => _isAi;
    public bool IsDead => _entityHealthControl.IsDead;

    public void SetWorkState(bool isCanWork) => CanComponentsWork = isCanWork;
}