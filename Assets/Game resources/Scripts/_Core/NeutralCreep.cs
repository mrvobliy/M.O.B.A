using UnityEngine;

public class NeutralCreep : Unit
{
	[Header("Neutral Creep")]
	[SerializeField] private float _passiveCooldown;

	private float _currentPassiveCooldown;
	private Vector3 _spawnPosition;

	protected void Awake()
	{
		base.Awake();

		_spawnPosition = transform.position;
		OnDamageTaken += Unit_OnDamageTaken;
	}

	private void Unit_OnDamageTaken()
	{
		_currentPassiveCooldown = _passiveCooldown;

		_targetToKill = FindClosestTarget();
	}

	protected override Vector3 GetTarget()
	{
		_currentPassiveCooldown = Mathf.MoveTowards(_currentPassiveCooldown, 0f, Time.deltaTime);
		if (Mathf.Approximately(_currentPassiveCooldown, 0f))
		{
			_targetToKill = null;
			_agent.stoppingDistance = _attackDistance;
			return _spawnPosition;
		}

		if (_targetToKill != null)
		{
			_agent.stoppingDistance = _attackDistance;
			return _targetToKill.transform.position;
		}
		else
		{
			_agent.stoppingDistance = 0f;
			return _spawnPosition;
		}
	}

	protected override bool IsTargetValid()
	{
		return true;
	}
}
