using UnityEngine;

public class UserInterface : MonoBehaviour
{
	[SerializeField] private Transform _healthbarParent;

	private void Awake()
	{
		Target.OnCreateHealthBar += OnAttackTargetAwake;
	}

	private void OnAttackTargetAwake(Healthbar healthBarPrefab, Target target)
	{
		if (target.DontCreateHealthBar || healthBarPrefab == null) return;
		
		var healthBar = Instantiate(healthBarPrefab, _healthbarParent);
		healthBar.Init(target);
	}
}
