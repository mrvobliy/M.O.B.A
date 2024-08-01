using System.Collections;
using System.Collections.Generic;
using EmeraldAI;
using UnityEngine;

public class LineEnemySpawn : MonoBehaviour
{
    [SerializeField] private float _spawnDelay;
    
    [SerializeField] private EmeraldAISystem _meeleEnemyPrefab;
    [SerializeField] private EmeraldAISystem _rangeEnemyPrefab;
    
    [SerializeField] private List<Transform> _topLinePoints;
    [SerializeField] private List<Transform> _midleLinePoints;
    [SerializeField] private List<Transform> _downLinePoints;

    [SerializeField] private EmeraldAIWaypointObject _topLineWay;
    [SerializeField] private EmeraldAIWaypointObject _midleLineWay;
    [SerializeField] private EmeraldAIWaypointObject _downLineWay;

    private Coroutine _onSpawnCoroutine;
    
    private void Start()
    {
        _onSpawnCoroutine = StartCoroutine(OnSpawn());
    }

    private IEnumerator OnSpawn()
    {
        var delay = new WaitForSeconds(_spawnDelay);
        
        while (true)
        {
            Spawn(_topLinePoints, _topLineWay);
            Spawn(_midleLinePoints, _midleLineWay);
            Spawn(_downLinePoints, _downLineWay);
            yield return delay;
        }
    }

    private void Spawn(List<Transform> spawnPoints, EmeraldAIWaypointObject way)
    {
        for (var i = 0; i < spawnPoints.Count; i++)
        {
            var spawnPrefab = i < 3 ? _meeleEnemyPrefab : _rangeEnemyPrefab;
            var newEnemy = Instantiate(spawnPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
            newEnemy.WaypointsList = way.Waypoints;
            newEnemy.gameObject.SetActive(true);
        }
    }
}
