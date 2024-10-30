using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class PlayerHero : Unit
{
	[Header("Player")] 
	[SerializeField] private float _speed;
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

	protected override bool IsTargetValid(Target target)
	{
		return true;
	}

	public void ActivateFirstSkill(SwordGirlFirstSkillControl swordGirlFirstSkillControl)
	{
		if (IsDead || _dontCanWork) return;
		
		_animator.SetTrigger(AnimatorHash.IsFirstSkill);
		_animator.DOLayerWeight(4, 1f, _blendAttackLayerDuration);
		_dontCanWork = true;
		var toRotation = Quaternion.LookRotation(swordGirlFirstSkillControl.FirstSkillDestinationPoint.position - _rotationParent.position, Vector3.up);
		_rotationParent.DORotate(toRotation.eulerAngles, 0.2f);

		StartCoroutine(OnActivate());
		
        IEnumerator OnActivate()
		{
			_agent.speed = 4;
			_agent.SetDestination(swordGirlFirstSkillControl.FirstSkillDestinationPoint.position);
			
			yield return new WaitForSeconds(0.8f);

			var skillDamage = Instantiate(swordGirlFirstSkillControl.FirstSkillDamagePrefab, swordGirlFirstSkillControl.FirstSkillSpawnPoint);
			skillDamage.Init(this);
			skillDamage.gameObject.SetActive(true);
			skillDamage.gameObject.transform.SetParent(null);
			
			yield return new WaitForSeconds(2);
			
			_animator.DOLayerWeight(4, 0f, _blendAttackLayerDuration);
			_agent.speed = _speed;
			_dontCanWork = false;
		}
	}
	
	public void ActivateSecondSkill(Action animCast = null)
	{
		if (IsDead || _dontCanWork) return;
		
		_animator.SetTrigger(AnimatorHash.IsSecondSkill);
		_animator.DOLayerWeight(4, 1f, _blendAttackLayerDuration);
		_dontCanWork = true;

		StartCoroutine(OnActivate());
		
		IEnumerator OnActivate()
		{
			yield return new WaitForSeconds(0.3f);
			
			animCast?.Invoke();
			
			yield return new WaitForSeconds(1f);
			
			_animator.DOLayerWeight(5, 0f, _blendAttackLayerDuration);
			_dontCanWork = false;
		}
	}
	
	public void ActivateThirdSkill(Action animCast = null)
	{
		if (IsDead || _dontCanWork) return;
		
		_animator.SetTrigger(AnimatorHash.IsThirdSkill);
		_animator.DOLayerWeight(4, 1f, _blendAttackLayerDuration);
		_dontCanWork = true;

		StartCoroutine(OnActivate());
		
		IEnumerator OnActivate()
		{
			yield return new WaitForSeconds(0.3f);
			
			animCast?.Invoke();
			
			yield return new WaitForSeconds(1f);
			
			_animator.DOLayerWeight(5, 0f, _blendAttackLayerDuration);
			_dontCanWork = false;
		}
	}
}
