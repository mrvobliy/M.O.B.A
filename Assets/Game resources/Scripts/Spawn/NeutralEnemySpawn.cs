using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralEnemySpawn :MonoBehaviour
{
    [SerializeField] private GameObject _meleeEnemyPrefab;
    [SerializeField] private GameObject _rangeEnemyPrefab;
    
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private List<EntityComponentsData> _spawnedList;
    [SerializeField] private float _newSpawnDelay;

    private int _countActiveEnemy;

    private void Start() => Spawn();

    private void Spawn()
    {
        foreach (var entityComponent in _spawnedList) 
            entityComponent.OnDeathStart -= EnemyDeath;
        
        _spawnedList.Clear();

        for (var i = 0; i < _spawnPoints.Count; i++)
        {
            var spawnPrefab = i < 3 ? _meleeEnemyPrefab : _rangeEnemyPrefab;
            var newEnemy = Instantiate(spawnPrefab, _spawnPoints[i].position, _spawnPoints[i].rotation);
            var entityComponent = newEnemy.GetComponentInChildren<EntityComponentsData>();
            entityComponent.CreepMoveControl.SetSpawnPoint(_spawnPoints[i]);
            entityComponent.OnDeathStart += EnemyDeath;
            _spawnedList.Add(entityComponent);
            _countActiveEnemy++; 
        }
    }

    private void EnemyDeath()
    {
        _countActiveEnemy--;
        
        if (_countActiveEnemy <= 0)
            StartCoroutine(OnDelaySpawnCoroutine());
    }

    private IEnumerator OnDelaySpawnCoroutine()
    {
        yield return new WaitForSeconds(_newSpawnDelay);
        Spawn();
    }
}