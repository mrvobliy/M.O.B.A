using UnityEngine;

public class NeutralCreep : Creep
{
	[Header("Neutral Creep")]
	[SerializeField] private float _passiveCooldown;

	private float _currentPassiveCooldown;
	private Vector3 _spawnPosition;

	protected new void Awake()
	{
		base.Awake();

		_spawnPosition = transform.position;
		OnHealthChanged += Unit_OnDamageTaken;
	}

	private void Unit_OnDamageTaken()
	{
		_currentPassiveCooldown = _passiveCooldown;

		_closestEnemyInVisibility = FindClosestEnemyInVisibilityRadius();

		_aggressive = true;
	}

	protected override Vector3 GetTarget()
	{
		_currentPassiveCooldown = Mathf.MoveTowards(_currentPassiveCooldown, 0f, Time.deltaTime);
		if (Mathf.Approximately(_currentPassiveCooldown, 0f))
		{
			_aggressive = false;
			_closestEnemyInVisibility = null;
			_agent.stoppingDistance = _attackDistance;
			return _spawnPosition;
		}

		if (_closestEnemyInVisibility != null)
		{
			_agent.stoppingDistance = _attackDistance;
			return _closestEnemyInVisibility.transform.position;
		}
		else
		{
			_agent.stoppingDistance = 0f;
			return _spawnPosition;
		}
	}

	protected override bool IsTargetValid(Target target) => target is PlayerHero;
}
