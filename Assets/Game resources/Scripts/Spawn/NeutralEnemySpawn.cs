using System.Collections.Generic;
using UnityEngine;

public class NeutralEnemySpawn : MonoBehaviour
{
    [SerializeField] private NeutralCreep _meeleEnemyPrefab;
    [SerializeField] private NeutralCreep _rangeEnemyPrefab;
    [SerializeField] private List<Transform> _spawnPoints;

    private int _countActiveEnemy;
    
    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        for (var i = 0; i < _spawnPoints.Count; i++)
        {
            var spawnPrefab = i < 3 ? _meeleEnemyPrefab : _rangeEnemyPrefab;
            var newEnemy = Instantiate(spawnPrefab, _spawnPoints[i].position, Quaternion.identity);
            newEnemy.SetRotation(_spawnPoints[i].rotation);
            _countActiveEnemy++;
        }
    }

    private void EnemyDeath()
    {
        _countActiveEnemy--;
        
        if (_countActiveEnemy > 0) return;
        
        Spawn();
    }
}
