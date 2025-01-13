using UnityEngine;

public class SwordGirlSecondSkillControl : MonoBehaviour
{
    [SerializeField] private PlayerHero _playerHero;
    [SerializeField] private EntityComponentsData _entityComponentsData;
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
        if (_playerHero.DontCanWork) return;
        
        _playerHero.ActivateSecondSkill(() =>
        {
            var skillDamage = Instantiate(_damagePrefab, _spawnPoint);
            skillDamage.Init(_entityComponentsData);
            skillDamage.gameObject.SetActive(true);
            skillDamage.gameObject.transform.SetParent(null);
        });
    }
}
