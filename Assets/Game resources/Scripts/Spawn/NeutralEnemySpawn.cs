using System.Collections.Generic;
using EmeraldAI;
using UnityEngine;

public class NeutralEnemySpawn : MonoBehaviour
{
    [SerializeField] private EmeraldAISystem _meeleEnemyPrefab;
    [SerializeField] private EmeraldAISystem _rangeEnemyPrefab;
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
            var newEnemy = Instantiate(spawnPrefab, _spawnPoints[i].position, _spawnPoints[i].rotation);
            newEnemy.gameObject.SetActive(true);
            newEnemy.DeathEvent.AddListener(EnemyDeath);
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
