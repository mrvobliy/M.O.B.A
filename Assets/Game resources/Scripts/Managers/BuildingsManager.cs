using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingsManager : SerializedMonoBehaviour
{
    [SerializeField] private List<GameObject> _lightSideBuildingsGameObjects;
    [SerializeField] private List<GameObject> _darkSideBuildingsGameObjects;
    [Space]
    [SerializeField] private List<EntityComponentsData> _lightSideBuildings;
    [SerializeField] private List<EntityComponentsData> _darkSideBuildings;
    [Space]
    [SerializeField] private List<BusyBuild> _lightSideBusyBuildings;
    [SerializeField] private List<BusyBuild> _darkSideBusyBuildings;
    
    public static BuildingsManager Instance;

    private void Awake()
    {
        Instance = this;
        _lightSideBusyBuildings = new List<BusyBuild>();
        _darkSideBusyBuildings = new List<BusyBuild>();
    }

    public void LeftBuilding(EntityComponentsData componentsData)
    {
        var buildingsList = GetBusyBuildList(componentsData.EntityTeam);

        foreach (var building in buildingsList.Where(building => building.Build == componentsData))
        {
            buildingsList.Remove(building);
            break;
        }
    }
    
    public EntityComponentsData GetNearestNotBusyBuild(EntityComponentsData componentsData)
    {
        var busyBuild = TryReturnBusyBuild(componentsData);

        if (busyBuild != null)
            return busyBuild;

        var buildings = GetBuildList(componentsData.EntityTeam);
        var busyBuildings = GetBusyBuildList(componentsData.EntityTeam);
        
        foreach (var building in buildings)
        {
            if (building.EntityHealthControl.IsDead) continue;
            
            var newBusyBuild = new BusyBuild(building, componentsData);
            busyBuildings.Add(newBusyBuild);
                
            var deadBuildings = buildings.Where(x => x.EntityHealthControl.IsDead).ToList();

            foreach (var deadBuilding in deadBuildings) 
                buildings.Remove(deadBuilding);
                
            return building;
        }
        
        return null;
    }
    
    private EntityComponentsData TryReturnBusyBuild(EntityComponentsData componentsData)
    {
        var buildingsList = GetBusyBuildList(componentsData.EntityTeam);

        foreach (var component in buildingsList)
        {
            if (component.EntityData != componentsData) 
                continue;

            if (!component.Build.EntityHealthControl.IsDead) 
                return component.Build;
            
            buildingsList.Remove(component);
            return null;
        }
        
        var deadBuildings = buildingsList.Where(x => x.Build.EntityHealthControl.IsDead).ToList();

        foreach (var building in deadBuildings) 
            buildingsList.Remove(building);
        
        return null;
    }
    
    
    public EntityComponentsData GetNearestRandomBuild(Team team)
    {
        var buildings = team == Team.Light ? _lightSideBuildings : _darkSideBuildings;
        var nearestBuild = new List<EntityComponentsData>();
        var index = 0;

        foreach (var building in buildings)
        {
            if (building.EntityHealthControl.IsDead) continue;

            nearestBuild.Add(building);
            index++;
            
            if (index > 2) 
                break;
        }

        var randomIndex = Random.Range(0, 3);

        return nearestBuild[randomIndex];
    }
    
    private List<BusyBuild> GetBusyBuildList(Team team) => team == Team.Light ? _darkSideBusyBuildings : _lightSideBusyBuildings;
    
    private List<EntityComponentsData> GetBuildList(Team team) => team == Team.Light ? _darkSideBuildings : _lightSideBuildings;

    [Button]
    private void SetBuildings()
    {
        _lightSideBuildings = _lightSideBuildingsGameObjects
            .Select(x => x.GetComponentInChildren<EntityComponentsData>()).ToList();
        
        _darkSideBuildings = _darkSideBuildingsGameObjects
            .Select(x => x.GetComponentInChildren<EntityComponentsData>()).ToList();
    }
}

public class BusyBuild
{
    public EntityComponentsData Build;
    public EntityComponentsData EntityData;

    public BusyBuild(EntityComponentsData build, EntityComponentsData entity)
    {
        Build = build;
        EntityData = entity;
    }
}