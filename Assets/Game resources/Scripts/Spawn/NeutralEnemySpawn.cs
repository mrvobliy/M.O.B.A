using System.Collections.Generic;
using EmeraldAI;
using UnityEngine;

public class NeutralEnemySpawn : MonoBehaviour
{
    [SerializeField] private Unit _meeleEnemyPrefab;
    [SerializeField] private Unit _rangeEnemyPrefab;
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
            newEnemy.Init(null, _spawnPoints[i].rotation);
            //newEnemy.DeathEvent.AddListener(EnemyDeath);
            // TODO
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
