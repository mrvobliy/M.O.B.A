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

    private void Start()
    {
        Spawn();
        /*var newEnemy = Instantiate(_meleeEnemyPrefab, _spawnPoints[0].position, _spawnPoints[0].rotation);
        var entityComponent = newEnemy.GetComponentInChildren<EntityComponentsData>();
        entityComponent.CreepMoveControl.SetSpawnPoint(_spawnPoints[0]);*/
    }

    private void Spawn()
    {
        _spawnedList.Clear();

        for (var i = 0; i < _spawnPoints.Count; i++)
        {
            var spawnPrefab = i < 3 ? _meleeEnemyPrefab : _rangeEnemyPrefab;
            var newEnemy = Instantiate(spawnPrefab, _spawnPoints[i].position, _spawnPoints[i].rotation);
            var entityComponent = newEnemy.GetComponentInChildren<EntityComponentsData>();
            entityComponent.CreepMoveControl.SetSpawnPoint(_spawnPoints[i]);
            entityComponent.EntityHealthControl.OnDeathStart += EnemyDeath;
            _spawnedList.Add(entityComponent);
            _countActiveEnemy++;
        }
    }

    private void EnemyDeath()
    {
        foreach (var enemy in _spawnedList)
        {
            if (!enemy.EntityHealthControl.IsDead) 
                continue;
            
            enemy.EntityHealthControl.OnDeathStart -= EnemyDeath;
        }

        _countActiveEnemy--;

        if (_countActiveEnemy > 0) 
            return;

        StartCoroutine(OnDelaySpawnCoroutine());
    }

    private IEnumerator OnDelaySpawnCoroutine()
    {
        yield return new WaitForSeconds(_newSpawnDelay);
        Spawn();
    }
}