using UnityEngine;
using UnityEngine.AI;

public abstract class EntityMoveControl : MonoBehaviour
{
    [SerializeField] protected EntityComponentsData _entityComponentsData;
    [SerializeField] protected NavMeshAgent _agent;
    [SerializeField] private float _rotationSpeed = 200f;

    private Animator Animator => _entityComponentsData.EntityHealthControl.Animator;
    private bool IsDead => _entityComponentsData.EntityHealthControl.IsDead;
    private Transform RotationParent => _entityComponentsData.EntityHealthControl.RotationParent;
    
    private bool _dontCanWork;
    
    public void SetRotation(Quaternion rotation) => RotationParent.rotation = rotation;

    protected virtual void Update()
    {
        if (IsDead || _dontCanWork) 
            return;

        var speed = _agent.velocity.magnitude;

        Animator.SetBool(AnimatorHash.IsRunning, speed > 0.01f);
        Animator.SetFloat(AnimatorHash.Speed, speed);

        var target = GetTarget();
        _agent.SetDestination(target);
        var direction = target.SetY(0f) - transform.position.SetY(0f);

        if (direction.sqrMagnitude <= 0.01f)
            return;
        
        var fromRotation =  RotationParent.rotation;
        var toRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        RotationParent.rotation = Quaternion.RotateTowards(fromRotation, toRotation, Time.deltaTime * _rotationSpeed);
    }
    
    protected abstract Vector3 GetTarget();
}
