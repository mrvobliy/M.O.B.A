using UnityEngine;
using UnityEngine.AI;

public class PlayerHero : Unit
{
	[Header("Player")]
	[SerializeField] private float _destinationScale;
	[SerializeField] private float _sampleScale;
	[SerializeField] private float _sampleDistance;
	[SerializeField] private float _blendAttackLayerDuration = 0.3f;

	private bool _blendAttack;

	protected override Vector3 GetTarget()
	{
		if (_agent.velocity.magnitude < 0.1f)
		{
			if (_blendAttack)
			{
				_blendAttack = false;
				_animator.DOLayerWeight(2, 0f, _blendAttackLayerDuration);
				_animator.DOLayerWeight(3, 1f, _blendAttackLayerDuration);
			}
		}
		else
		{
			if (_blendAttack == false)
			{
				_blendAttack = true;
				_animator.DOLayerWeight(2, 1f, _blendAttackLayerDuration);
				_animator.DOLayerWeight(3, 0f, _blendAttackLayerDuration);
			}
		}

		_agent.stoppingDistance = 0f;

		var x = Joystick.Instance.Direction.x;
		var y = Joystick.Instance.Direction.y;
		var inputDirection = new Vector3(x, 0f, y);

		if (inputDirection.magnitude < 0.01f)
		{
			return transform.position;
		}

		var sampled = NavMesh.SamplePosition(transform.position + inputDirection.normalized * _sampleScale,
			out var hit, _sampleDistance, NavMesh.AllAreas);

		if (sampled == false)
		{
			return transform.position;
		}

		return transform.position + inputDirection.normalized * _destinationScale;
	}

	protected override bool IsTargetValid()
	{
		return true;
	}
}
