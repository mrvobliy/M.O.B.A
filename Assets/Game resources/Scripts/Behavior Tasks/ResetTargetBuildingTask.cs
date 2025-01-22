using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ResetTargetBuildingTask : Action
{
    [SerializeField] private EntityComponentsData _entityComponentsData;

    public override void OnStart()
    {
        base.OnStart();
        BuildingsManager.Instance.SetLeftBusyBuilding(_entityComponentsData, false);
    }
}