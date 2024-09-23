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
		OnDamageTaken += Unit_OnDamageTaken;
	}

	private void Unit_OnDamageTaken()
	{
		_currentPassiveCooldown = _passiveCooldown;

		_closestEnemy = FindClosestTarget();
	}

	protected override Vector3 GetTarget()
	{
		_currentPassiveCooldown = Mathf.MoveTowards(_currentPassiveCooldown, 0f, Time.deltaTime);
		if (Mathf.Approximately(_currentPassiveCooldown, 0f))
		{
			_closestEnemy = null;
			_agent.stoppingDistance = _attackDistance;
			return _spawnPosition;
		}

		if (_closestEnemy != null)
		{
			_agent.stoppingDistance = _attackDistance;
			return _closestEnemy.transform.position;
		}
		else
		{
			_agent.stoppingDistance = 0f;
			return _spawnPosition;
		}
	}

	protected override bool IsTargetValid(Target target)
	{
		return true;
	}
}
