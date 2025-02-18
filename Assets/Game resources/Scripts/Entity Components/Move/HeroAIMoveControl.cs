using UnityEngine;

public class HeroAIMoveControl : EntityMoveControl
{
    private const float OffsetStoppingDistance = 2;
    private const float BlendAttackLayerDuration = 0.3f;
    
    [SerializeField] private float _sampleScale;
    [SerializeField] private float _sampleDistance;
    [SerializeField] private float _destinationScale;
    
    private bool _blendAttack;
    private Transform _target;
    
    private void OnEnable()
    {
        _componentsData.NavMeshAgent.enabled = _componentsData.IsAi;
        enabled = _componentsData.IsAi;
    }
    
    protected override Vector3 GetTarget()
    {
        SetAnimatorLayers();
        
        _componentsData.NavMeshAgent.stoppingDistance = 0f;

        if (_target == null) 
            return transform.position;
        
        _componentsData.NavMeshAgent.stoppingDistance = _componentsData.EntityAttackControl.AttackDistance / OffsetStoppingDistance;
        return _target.position;
    }

    public void SetAiTarget(Transform target) => _target = target;

    private void SetAnimatorLayers()
    {
        if (_componentsData.NavMeshAgent.velocity.magnitude < 0.1f)
        {
            if (!_blendAttack) return;
            
            _blendAttack = false;
            _componentsData.Animator.DOLayerWeight(2, 0f, BlendAttackLayerDuration);
            _componentsData.Animator.DOLayerWeight(3, 1f, BlendAttackLayerDuration);
        }
        else
        {
            if (_blendAttack) return;
            
            _blendAttack = true;
            _componentsData.Animator.DOLayerWeight(2, 1f, BlendAttackLayerDuration);
            _componentsData.Animator.DOLayerWeight(3, 0f, BlendAttackLayerDuration);
        }
    }
}