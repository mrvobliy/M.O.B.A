using UnityEngine;

public class SwordGirlSecondSkillControl : MonoBehaviour
{
    [SerializeField] private PlayerHero _playerHero;
    [SerializeField] private ButtonEvents _skillButtonEvents;
    [SerializeField] private PlayerSkillDamage _damagePrefab;
    [SerializeField] private Transform _spawnPoint;

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
        if (_playerHero.IsSkillEnable) return;
        
        _playerHero.ActivateSecondSkill(() =>
        {
            var skillDamage = Instantiate(_damagePrefab, _spawnPoint);
            skillDamage.Init(_playerHero);
            skillDamage.gameObject.SetActive(true);
            skillDamage.gameObject.transform.SetParent(null);
        });
    }
}
