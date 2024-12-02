using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralEnemySpawn :MonoBehaviour
{
    [SerializeField] private NeutralCreep _meeleEnemyPrefab;
    [SerializeField] private NeutralCreep _rangeEnemyPrefab;
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private List<NeutralCreep> _spawnedList;
    [SerializeField] private float _newSpawnDelay;

    private int _countActiveEnemy;

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        _spawnedList.Clear();

        for (var i = 0; i < _spawnPoints.Count; i++)
        {
            var spawnPrefab = i < 3 ? _meeleEnemyPrefab : _rangeEnemyPrefab;
            var newEnemy = Instantiate(spawnPrefab, _spawnPoints[i].position, Quaternion.identity);
            newEnemy.SetRotation(_spawnPoints[i].rotation);
            newEnemy.OnDeath += EnemyDeath;
            _spawnedList.Add(newEnemy);
            _countActiveEnemy++;
        }
    }

    private void EnemyDeath()
    {
        foreach (var enemy in _spawnedList)
        {
            if (!enemy.IsDead) continue;

            enemy.OnDeath -= EnemyDeath;
        }

        _countActiveEnemy--;

        if (_countActiveEnemy > 0) return;

        StartCoroutine(OnDelaySpawnCoroutine());
    }

    private IEnumerator OnDelaySpawnCoroutine()
    {
        yield return new WaitForSeconds(_newSpawnDelay);
        Spawn();
    }
}
