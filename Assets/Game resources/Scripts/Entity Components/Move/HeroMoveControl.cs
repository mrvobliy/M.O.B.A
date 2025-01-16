using UnityEngine;

public class HeroMoveControl : EntityMoveControl
{
    [SerializeField] private float _blendAttackLayerDuration = 0.3f;
    [SerializeField] private float _sampleScale;
    [SerializeField] private float _sampleDistance;
    [SerializeField] private float _destinationScale;
    
    private bool _blendAttack;
    
    protected override Vector3 GetTarget()
    {
        if (_agent.velocity.magnitude < 0.1f)
        {
            if (_blendAttack)
            {
                _blendAttack = false;
                Animator.DOLayerWeight(2, 0f, _blendAttackLayerDuration);
                Animator.DOLayerWeight(3, 1f, _blendAttackLayerDuration);
            }
        }
        else
        {
            if (_blendAttack == false)
            {
                _blendAttack = true;
                Animator.DOLayerWeight(2, 1f, _blendAttackLayerDuration);
                Animator.DOLayerWeight(3, 0f, _blendAttackLayerDuration);
            }
        }

        _agent.stoppingDistance = 0f;

        var x = Joystick.Instance.Direction.x;
        var y = Joystick.Instance.Direction.y;
        var inputDirection = new Vector3(x, 0f, y);
		
        if (inputDirection.magnitude < 0.01f)
            return transform.position;

        var sampled = UnityEngine.AI.NavMesh.SamplePosition(transform.position + inputDirection.normalized * _sampleScale,
            out var hit, _sampleDistance, UnityEngine.AI.NavMesh.AllAreas);

        if (sampled == false)
            return transform.position;

        return transform.position + inputDirection.normalized * _destinationScale;
    }
}