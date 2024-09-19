using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineEnemySpawn : MonoBehaviour
{
    [SerializeField] private float _spawnDelay;
    
    [SerializeField] private LaneCreep _meeleEnemyPrefab;
    [SerializeField] private LaneCreep _rangeEnemyPrefab;
    
    [SerializeField] private List<Transform> _topLinePoints;
    [SerializeField] private List<Transform> _midleLinePoints;
    [SerializeField] private List<Transform> _downLinePoints;

    [SerializeField] private Transform[] _topLineWay;
    [SerializeField] private Transform[] _midleLineWay;
    [SerializeField] private Transform[] _downLineWay;

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

    private void Spawn(List<Transform> spawnPoints, Transform[] way)
    {
        for (var i = 0; i < spawnPoints.Count; i++)
        {
            var spawnPrefab = i < 3 ? _meeleEnemyPrefab : _rangeEnemyPrefab;
            var newEnemy = Instantiate(spawnPrefab, spawnPoints[i].position, Quaternion.identity);
            newEnemy.SetWaypoints(way);
            newEnemy.SetRotation(spawnPoints[i].rotation);
		}
    }
}
