using UnityEngine;
using UnityEngine.UI;

public class HealthbarView : MonoBehaviour
{
	[SerializeField] private Image _fill;
	[SerializeField] private Vector3 _offset;

	private AttackTarget _target;

	public void Init(AttackTarget target)
	{
		_target = target;

		_fill.fillAmount = _target.HealthPercent;
		_target.OnDeath += OnDeath;
		_target.OnDamageTaken += OnDamageTaken;
	}

	private void OnDamageTaken()
	{
		_fill.fillAmount = _target.HealthPercent;
	}

	private void OnDeath()
	{
		_target.OnDeath -= OnDeath;
		_target.OnDamageTaken -= OnDamageTaken;

		Destroy(gameObject);
	}

	private void LateUpdate()
	{
		var position = _target.transform.position + _offset;
		var screenPosition = Camera.main.WorldToScreenPoint(position);
		screenPosition.z = 0f;

		transform.position = screenPosition;
	}
}
