using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKPositionController : MonoBehaviour
{
    public IKWeightController ikWeightController;
    public IKController ikController;
    public Transform upperTarget;
    public Transform lowerTarget;

    private void MoveToTargetIK(Transform target, float time, bool start)
    {
        if (ikController != null && ikWeightController != null)
        {
            ikController.targetTransform = target;
            if (start)
                ikWeightController.StartIK(time);
            else
                ikWeightController.StopIK(time);
        }
    }
    
    public void StartUpperIK(float time) => MoveToTargetIK(upperTarget, time, true);
    public void StartLowerIK(float time) => MoveToTargetIK(lowerTarget, time, true);
    public void StopUpperIK(float time) => MoveToTargetIK(upperTarget, time, false);
    public void StopLowerIK(float time) => MoveToTargetIK(lowerTarget, time, false);
}
