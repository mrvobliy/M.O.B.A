using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
	[SerializeField] private Image _fill;
	[SerializeField] private Vector3 _offset;

	private Target _target;

	public void Init(Target target)
	{
		_target = target;

		_fill.fillAmount = _target.HealthPercent;
		_target.OnDeath += OnDeath;
		_target.OnDamageTaken += OnDamageTaken;

		_fill.color = _target.Team switch
		{
			Team.Neutral => Color.yellow,
			Team.Light => Color.green,
			Team.Dark => Color.red
		};
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
