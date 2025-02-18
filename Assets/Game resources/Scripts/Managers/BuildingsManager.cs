using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingsManager : SerializedMonoBehaviour
{
    [SerializeField] private List<TowersLine> _lightSideLines;
    [SerializeField] private List<TowersLine> _darkSideLines;
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

    public void SetLeftBusyBuilding(EntityComponentsData componentsData, bool isBusy, EntityComponentsData newBusyBuilding = null)
    {
        var busyBuildings = GetBusyBuildList(componentsData.EntityTeam);

        if (!isBusy)
        {
            foreach (var building in busyBuildings.Where(building => building.EntityData == componentsData))
            {
                busyBuildings.Remove(building);
                break;
            }
        }
        else if (newBusyBuilding != null)
        {
            var newBusyBuild = new BusyBuild(newBusyBuilding, componentsData);
            busyBuildings.Add(newBusyBuild);
        }
    }
    
    public EntityComponentsData GetNearestNotBusyBuild(EntityComponentsData componentsData)
    {
        var busyBuild = TryReturnBusyBuild(componentsData);

        if (busyBuild != null)
            return busyBuild;
        
        var towersLines = GetBuildList(componentsData.EntityTeam);

        var currentLineIndex = 0;
        
        while (currentLineIndex < 3)
        {
            var line = towersLines[currentLineIndex];
            var tower = line.Towers.FirstOrDefault(tower => 
                tower != null && !tower.ComponentsData.IsDead);

            if (IsBuildingBusy(tower.ComponentsData, componentsData.EntityTeam))
                tower = null;
            
            currentLineIndex++;

            if (tower == null) 
                continue;
            
            SetLeftBusyBuilding(componentsData, true, tower.ComponentsData);
            return tower.ComponentsData;
        }

        var throne = _lightSideLines[3].Towers.FirstOrDefault(x => !x.ComponentsData.IsDead);
        return throne.ComponentsData;
    }
    
    public EntityComponentsData GetNearestRandomBuild(EntityComponentsData componentsData)
    {
        var busyBuild = TryReturnBusyBuild(componentsData);

        if (busyBuild != null)
            return busyBuild;
        
        var towersLine = GetBuildList(componentsData.EntityTeam);
        
        var gotLineIndexes = new List<int>();
        
        while (gotLineIndexes.Count < 3)
        {
            var randomLineIndex = Random.Range(0, 3);
            
            if (gotLineIndexes.Contains(randomLineIndex))
                continue;
                
            gotLineIndexes.Add(randomLineIndex);
            
            var line = towersLine[randomLineIndex];
            var tower = line.Towers.FirstOrDefault(x => x != null && !x.ComponentsData.IsDead);

            if (tower == null) 
                continue;
            
            SetLeftBusyBuilding(componentsData, true, tower.ComponentsData);
            return tower.ComponentsData;
        }

        var throne = _lightSideLines[3].Towers.FirstOrDefault(x => !x.ComponentsData.IsDead);
        return throne.ComponentsData;
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

    private bool IsBuildingBusy(EntityComponentsData buildingData, Team team)
    {
        var buildings = GetBusyBuildList(team);
        var build = buildings.FirstOrDefault(x => x.Build == buildingData);
        return build != null;
    }
    
    private List<BusyBuild> GetBusyBuildList(Team team) => team == Team.Light ? _darkSideBusyBuildings : _lightSideBusyBuildings;
    private List<TowersLine> GetBuildList(Team team) => team == Team.Light ? _darkSideLines : _lightSideLines;
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

public class TowersLine
{
    public List<TowerComponentsData> Towers;
}