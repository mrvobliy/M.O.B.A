using System.Collections;
using UnityEngine;

public abstract class EntityMoveControl : MonoBehaviour
{
    [SerializeField] protected EntityComponentsData _componentsData;
    [SerializeField] private float _rotationSpeed = 200f;
    [SerializeField] private int _stunLayerIndex = 2;
    
    private bool IsDead => _componentsData.EntityHealthControl.IsDead;
    
    public void SetRotation(Quaternion rotation) => _componentsData.RotationRoot.rotation = rotation;

    protected virtual void Update()
    {
        if (IsDead || !_componentsData.CanComponentsWork) 
            return;

        var speed = _componentsData.NavMeshAgent.velocity.magnitude;

        _componentsData.Animator.SetBool(AnimatorHash.IsRunning, speed > 0.01f);
        _componentsData.Animator.SetFloat(AnimatorHash.Speed, speed);

        var target = GetTarget();
        _componentsData.NavMeshAgent.SetDestination(target);
        var direction = target.SetY(0f) - transform.position.SetY(0f);

        if (direction.sqrMagnitude <= 0.01f)
            return;
        
        var fromRotation =  _componentsData.RotationRoot.rotation;
        var toRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        _componentsData.RotationRoot.rotation = Quaternion.RotateTowards(fromRotation, toRotation, Time.deltaTime * _rotationSpeed);
    }
    
    protected abstract Vector3 GetTarget();
    
    public void TryStun(int percentChanceStun, float timeStun)
    {
        if (IsDead || !_componentsData.CanComponentsWork) 
            return;
		
        if (percentChanceStun < Random.Range(0, 101))
            return;

        StartCoroutine(OnStan(timeStun));
    }

    private IEnumerator OnStan(float timeStun)
    {
        var stunDelay = new WaitForSeconds(timeStun);
		
        _componentsData.NavMeshAgent.ResetPath();
        _componentsData.SetComponentsWorkState(false);
        _componentsData.Animator.DOLayerWeight(_stunLayerIndex, 1f, 0.3f);
        _componentsData.Animator.SetBool(AnimatorHash.IsStun, true);

        var effectPrefab = GameResourcesBase.Instance.StunEffect;
        GameObject stunEffect = null;
		
        if (effectPrefab != null)
            stunEffect = Instantiate(effectPrefab, _componentsData.RotationRoot);
		
        yield return stunDelay;
		
        _componentsData.SetComponentsWorkState(true);
        _componentsData.Animator.SetBool(AnimatorHash.IsStun, false);
        _componentsData.Animator.DOLayerWeight(_stunLayerIndex, 0f, 0.3f);
        Destroy(stunEffect);
    }
}