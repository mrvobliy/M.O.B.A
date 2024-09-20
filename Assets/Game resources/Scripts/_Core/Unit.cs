using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public abstract class Unit : Attacker
{
	[Header("Unit")]
	[SerializeField] protected NavMeshAgent _agent;
	[SerializeField] private float _rotationSpeed = 200f;

	public override float Radius => _agent.radius;

	public void SetRotation(Quaternion rotation)
	{
		_rotationParent.rotation = rotation;
	}

	protected void Awake()
	{
		base.Awake();

		_agent.updateRotation = false;

		OnDeath += Unit_OnDeath;

		_animator.SetFloat(AnimatorHash.Offset, Random.Range(0f, 1f));
	}

	private void Unit_OnDeath()
	{
		_agent.enabled = false;

		_animator.SetBool(AnimatorHash.IsRunning, false);
		_animator.SetFloat(AnimatorHash.Speed, 0f);
	}

	protected abstract Vector3 GetTarget();

	private void Update()
	{
		if (IsDead) return;

		var speed = _agent.velocity.magnitude;

		_animator.SetBool(AnimatorHash.IsRunning, speed > 0.01f);
		_animator.SetFloat(AnimatorHash.Speed, speed);

		var target = GetTarget();

		_agent.SetDestination(target);

		var direction = target.SetY(0f) - transform.position.SetY(0f);
		if (direction.sqrMagnitude > 0.1f)
		{
			var fromRotation = _rotationParent.rotation;
			var toRotation = Quaternion.LookRotation(direction, Vector3.up);
			_rotationParent.rotation = Quaternion.RotateTowards
				(fromRotation, toRotation, Time.deltaTime * _rotationSpeed);
		}
	}
}
