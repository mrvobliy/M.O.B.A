using UnityEngine;

public class SwordGirlSecondSkillControl : MonoBehaviour
{
    [SerializeField] private PlayerHero _playerHero;
    [SerializeField] private ButtonEvents _skillButtonEvents;
    [SerializeField] private PlayerSkillDamage _damagePrefab;
    [SerializeField] private Transform _spawnPoint;

    public PlayerSkillDamage DamagePrefab => _damagePrefab;
    public Transform SpawnPoint => _spawnPoint;

    private void OnEnable()
    {
        _skillButtonEvents.OnButtonDown += ReleaseSkill;
    }

    private void OnDisable()
    {
        _skillButtonEvents.OnButtonDown -= ReleaseSkill;
    }
    
    private void ReleaseSkill()
    {
        _playerHero.ActivateSecondSkill(this);
    }
}
