using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    [SerializeField] private EntitiesStatsDataBase _dataBase;
    [SerializeField] private GameResourcesData _gameResourcesData;

    public static GameDataManager Instance;
    
    public EntitiesStatsDataBase DataBase => _dataBase;
    public GameResourcesData GameResourcesData => _gameResourcesData;

    private void Awake() => Instance = this;
}