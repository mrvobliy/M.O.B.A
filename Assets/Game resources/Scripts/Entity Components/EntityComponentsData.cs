using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class EntityComponentsData : MonoBehaviour
{
    [Title("Properties")]
    [SerializeField] private Team _entityTeam;
    [SerializeField] private EntityType _entityType;
    [SerializeField, ShowIf(nameof(IsNeedShowGoldExpoParam))] private bool _isAi;
    
    private bool IsNeedShowTowerTier => _entityType is EntityType.Tower;
    [SerializeField, ShowIf(nameof(IsNeedShowTowerTier))] private TowerTier _towerTier;

    private bool IsNeedShowHeroInfo => _entityType is EntityType.Hero;
    [SerializeField, ShowIf(nameof(IsNeedShowHeroInfo))] private HeroInfo _heroInfo;
    
    private bool IsNeedShowStatsControl => _entityType is EntityType.Hero;
    [SerializeField, ShowIf(nameof(IsNeedShowStatsControl))] private HeroStatsControl _heroStatsControl;
    
    [Title("Controls")]
    [SerializeField] private EntityHealthControl _entityHealthControl;
    
    private bool IsNeedShowEntityMoveControl => _entityType is EntityType.Tower or EntityType.Throne;
    [SerializeField, HideIf(nameof(IsNeedShowEntityMoveControl))] private EntityMoveControl _entityMoveControl;
    
    private bool IsNeedShowAttackControl => _entityType != EntityType.Throne;
    [SerializeField, ShowIf(nameof(IsNeedShowAttackControl))] private EntityAttackControl _entityAttackControl;
    
    private bool IsNeedShowGoldExpoParam => _entityType == EntityType.Hero;
    [SerializeField, ShowIf(nameof(IsNeedShowGoldExpoParam))] private HeroGoldControl _heroGoldControl;
    [SerializeField, ShowIf(nameof(IsNeedShowGoldExpoParam))] private HeroExperienceControl _heroExperienceControl;
    
    private bool IsNeedShowCreepMoveControl => _entityType is EntityType.NeutralMelee or EntityType.NeutralRange;
    [SerializeField, ShowIf(nameof(IsNeedShowCreepMoveControl))] private CreepMoveControl _creepMoveControl;
    
    [Title("Components")]
    [SerializeField] private Collider _collider;
    [SerializeField] private Animator _animator;
    
    private bool IsNeedShowNavMeshAgent => _entityType is EntityType.Tower or EntityType.Throne;
    [SerializeField, HideIf(nameof(IsNeedShowNavMeshAgent))] private NavMeshAgent _navMeshAgent;
    
    private bool IsNeedCharacterController => _entityType == EntityType.Hero;
    [SerializeField, ShowIf(nameof(IsNeedCharacterController))] private CharacterController _characterController;
    
    private bool IsNeedShowNavMeshObstacle => _entityType is EntityType.Tower or EntityType.Throne;
    [SerializeField, ShowIf(nameof(IsNeedShowNavMeshObstacle))] private NavMeshObstacle _navMeshObstacle;
    
    [Title("Points")]
    [SerializeField] private Transform _rotationRoot;
    [SerializeField] private Transform _enemyAttackPoint;

    public Collider Collider => _collider;
    public NavMeshObstacle NavMeshObstacle => _navMeshObstacle;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public CharacterController CharacterController => _characterController;
    public Animator Animator => _animator;

    public Transform RotationRoot => _rotationRoot;
    public Transform EnemyAttackPoint => _enemyAttackPoint;
    
    public Team EntityTeam => _entityTeam;
    public TowerTier TowerTier => _towerTier;
    public EntityType EntityType => _entityType;
    public HeroInfo HeroInfo => _heroInfo;
    public HeroStatsControl HeroStatsControl => _heroStatsControl;
    
    public CreepMoveControl CreepMoveControl => _creepMoveControl;
    public HeroGoldControl HeroGoldControl => _heroGoldControl;
    public HeroExperienceControl HeroExperienceControl => _heroExperienceControl;
    public EntityHealthControl EntityHealthControl => _entityHealthControl;
    public EntityMoveControl EntityMoveControl => _entityMoveControl;
    public EntityAttackControl EntityAttackControl => _entityAttackControl;
    
    public bool CanComponentsWork { get; private set; } = true;
    public bool IsAi => _isAi;
    public bool IsDead => _entityHealthControl.IsDead;

    public Action OnRespawn;
    public Action OnDeathStart;
    public Action OnDeathEnd;
    
    public void SetComponentsWorkState(bool isCanWork) => CanComponentsWork = isCanWork;

    [Button]
    public void SetComponents()
    {
        var newCollider = transform.parent.GetComponentInChildren<Collider>();

        if (newCollider != null)
            _collider = newCollider;
        
        
        var newAnimator = transform.parent.GetComponentInChildren<Animator>();

        if (newAnimator != null)
            _animator = newAnimator;
        
        
        var newNavMeshAgent = transform.parent.GetComponentInChildren<NavMeshAgent>();

        if (newNavMeshAgent != null)
            _navMeshAgent = newNavMeshAgent;
        
        
        var newCharacterController = transform.parent.GetComponentInChildren<CharacterController>();

        if (newCharacterController != null)
            _characterController = newCharacterController;
        
        
        var newNavMeshObstacle = transform.parent.GetComponentInChildren<NavMeshObstacle>();

        if (newNavMeshObstacle != null)
            _navMeshObstacle = newNavMeshObstacle;
    }
}