using UnityEngine;

public class UserInterface : MonoBehaviour
{
	[SerializeField] private Healthbar _healthbarPrefab;
	[SerializeField] private Transform _healthbarParent;

	private void Awake()
	{
		Target.OnStart += OnAttackTargetAwake;
	}

	private void OnAttackTargetAwake(Target target)
	{
		var healthbar = Instantiate(_healthbarPrefab, _healthbarParent);
		healthbar.Init(target);
	}
}
