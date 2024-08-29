using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _speedChangeStates;

    public static PlayerAnimator Instance;
    
    private float _layer1TargetWeight;
    private float _layer2TargetWeight;
    private float _layer3TargetWeight;
    private float _layer4TargetWeight;

    private void Awake()
    {
        Instance = this;
    }

    private void LateUpdate()
    {
        GetTargetWeightAnimLayers();
    }

    private void GetTargetWeightAnimLayers()
    {
        if (!VariableBase.IsAttackState && !VariableBase.IsRunState)
        {
            SetTargetWeightAnimLayers(1, 0, 0 ,0);
            return;
        }
        
        if (!VariableBase.IsAttackState && VariableBase.IsRunState)
        {
            SetTargetWeightAnimLayers(0, 1, 0 ,0);
            return;
        }
        
        if (VariableBase.IsAttackState && !VariableBase.IsRunState)
        {
            SetTargetWeightAnimLayers(0, 0, 1,0);
            return;
        }
        
        if (VariableBase.IsAttackState && VariableBase.IsRunState)
        {
            SetTargetWeightAnimLayers(0, 1, 0,1);
        }
    }

    private void SetTargetWeightAnimLayers(float layer1Weight, float layer2Weight, float layer3Weight, float layer4Weight)
    {
        _layer1TargetWeight = Mathf.MoveTowards(_animator.GetLayerWeight(0), layer1Weight, Time.deltaTime * _speedChangeStates);
        _layer2TargetWeight = Mathf.MoveTowards(_animator.GetLayerWeight(1), layer2Weight, Time.deltaTime * _speedChangeStates);
        _layer3TargetWeight = Mathf.MoveTowards(_animator.GetLayerWeight(2), layer3Weight, Time.deltaTime * _speedChangeStates);
        _layer4TargetWeight = Mathf.MoveTowards(_animator.GetLayerWeight(3), layer4Weight, Time.deltaTime * _speedChangeStates);
        
        _animator.SetLayerWeight(0, _layer1TargetWeight);
        _animator.SetLayerWeight(1, _layer2TargetWeight);
        _animator.SetLayerWeight(2, _layer3TargetWeight);
        _animator.SetLayerWeight(3, _layer4TargetWeight);
    }

    public void TryPlayAttackAnim()
    {
        if (!VariableBase.IsAttackState) return;

        var indexAnim = Random.Range(0, 3);
        _animator.SetTrigger("AttackTrigger_" + indexAnim);
    }
}
