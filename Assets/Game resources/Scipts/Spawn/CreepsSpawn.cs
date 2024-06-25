using System.Collections;
using System.Collections.Generic;
using com.cyborgAssets.inspectorButtonPro;
using EmeraldAI;
using UnityEngine;

public class CreepsSpawn : MonoBehaviour
{
    [SerializeField] private List<EmeraldAISystem> _enemyPrefab;
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private float _spawnDelay;

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
            Spawn();
            yield return delay;
        }
    }

    [ProButton]
    private void Spawn()
    {
        var pointIndex = 0;

        foreach (var enemy in _enemyPrefab)
        {
            for (var i = 0; i < 4; i++)
            {
                var creep = Instantiate(enemy, _spawnPoints[pointIndex].position, Quaternion.identity);
                creep.Activate();
                pointIndex++;
            }
        }
    }
}
