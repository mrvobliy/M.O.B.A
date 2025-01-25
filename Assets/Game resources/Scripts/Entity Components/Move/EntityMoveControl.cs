using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class EntityMoveControl : MonoBehaviour
{
    [SerializeField] protected EntityComponentsData _entityComponentsData;
    [SerializeField] protected NavMeshAgent _agent;
    [SerializeField] private float _rotationSpeed = 200f;
    [SerializeField] private int _stunLayerIndex = 2;

    public NavMeshAgent Agent => _agent;
    
    protected Animator Animator => _entityComponentsData.EntityHealthControl.Animator;
    private bool IsDead => _entityComponentsData.EntityHealthControl.IsDead;
    protected Transform RotationParent => _entityComponentsData.EntityHealthControl.RotationParent;
    
    public void SetRotation(Quaternion rotation) => RotationParent.rotation = rotation;

    protected virtual void Update()
    {
        if (IsDead || !_entityComponentsData.CanComponentsWork) 
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
    
    public void TryStun(int percentChanceStun, float timeStun)
    {
        if (IsDead || !_entityComponentsData.CanComponentsWork) 
            return;
		
        if (percentChanceStun < Random.Range(0, 101))
            return;

        StartCoroutine(OnStan(timeStun));
    }

    private IEnumerator OnStan(float timeStun)
    {
        var stunDelay = new WaitForSeconds(timeStun);
		
        _agent.ResetPath();
        _entityComponentsData.SetWorkState(false);
        Animator.DOLayerWeight(_stunLayerIndex, 1f, 0.3f);
        Animator.SetBool(AnimatorHash.IsStun, true);

        var effectPrefab = GameResourcesBase.Instance.StunEffect;
        GameObject stunEffect = null;
		
        if (effectPrefab != null)
            stunEffect = Instantiate(effectPrefab, RotationParent);
		
        yield return stunDelay;
		
        _entityComponentsData.SetWorkState(true);
        Animator.SetBool(AnimatorHash.IsStun, false);
        Animator.DOLayerWeight(_stunLayerIndex, 0f, 0.3f);
        Destroy(stunEffect);
    }
}