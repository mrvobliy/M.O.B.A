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
        Agent.enabled = _entityComponentsData.IsAi;
        enabled = _entityComponentsData.IsAi;
    }
    
    protected override Vector3 GetTarget()
    {
        SetAnimatorLayers();
        
        _agent.stoppingDistance = 0f;

        if (_target == null) 
            return transform.position;
        
        _agent.stoppingDistance = _entityComponentsData.EntityAttackControl.AttackDistance / OffsetStoppingDistance;
        return _target.position;
    }

    public void SetAiTarget(Transform target) => _target = target;

    private void SetAnimatorLayers()
    {
        if (_agent.velocity.magnitude < 0.1f)
        {
            if (!_blendAttack) return;
            
            _blendAttack = false;
            Animator.DOLayerWeight(2, 0f, BlendAttackLayerDuration);
            Animator.DOLayerWeight(3, 1f, BlendAttackLayerDuration);
        }
        else
        {
            if (_blendAttack) return;
            
            _blendAttack = true;
            Animator.DOLayerWeight(2, 1f, BlendAttackLayerDuration);
            Animator.DOLayerWeight(3, 0f, BlendAttackLayerDuration);
        }
    }
}