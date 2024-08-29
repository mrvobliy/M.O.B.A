using EmeraldAI;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private EnemyDetector _enemyDetector;
    [SerializeField] private int _damageValue;
    
    public void SetDamage()
    {
        if (_enemyDetector.AvailableEnemy.Count <= 0) return;
        
        _enemyDetector.AvailableEnemy[0].AISystem.Damage(_damageValue, EmeraldAISystem.TargetType.Player, transform, 50);
    }
}
