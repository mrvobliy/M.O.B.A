using BehaviorDesigner.Runtime;
using UnityEngine;

public class SetAiBehaviorState : MonoBehaviour
{
    [SerializeField] private BehaviorTree _behaviorTree;
    [SerializeField] private EntityComponentsData _componentsData;
    
    public void OnEnable() => _behaviorTree.enabled = _componentsData.IsAi;
}