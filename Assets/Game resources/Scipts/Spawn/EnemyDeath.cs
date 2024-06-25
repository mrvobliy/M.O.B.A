using System.Collections;
using EmeraldAI;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    [SerializeField] private EmeraldAISystem _emeraldAISystem;
    [SerializeField] private float _timeDelay;

    private void OnEnable()
    {
        _emeraldAISystem.DeathEvent.AddListener(DeathResetState);
    }

    private void OnDisable()
    {
        _emeraldAISystem.DeathEvent.RemoveListener(DeathResetState);
    }

    private void DeathResetState()
    {
        StartCoroutine(OnReset());
    }
    
    private IEnumerator OnReset()
    {
        
        yield return new WaitForSeconds(_timeDelay);
        Destroy(gameObject);
    }
}
