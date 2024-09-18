using UnityEngine;

public class UserInterface : MonoBehaviour
{
	[SerializeField] private HealthbarView _healthbarPrefab;
	[SerializeField] private Transform _healthbarParent;

	private void Awake()
	{
		AttackTarget.OnAwake += OnAttackTargetAwake;
	}

	private void OnAttackTargetAwake(AttackTarget target)
	{
		var healthbar = Instantiate(_healthbarPrefab, _healthbarParent);
		healthbar.Init(target);
	}
}
