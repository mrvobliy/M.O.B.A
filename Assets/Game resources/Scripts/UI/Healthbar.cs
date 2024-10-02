using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
	[SerializeField] private Vector3 _offset;
	[SerializeField] private CanvasGroup _canvasGroup;
	[SerializeField] private RectTransform _heathLine;

	private Target _target;

	public void Init(Target target)
	{
		_target = target;
		_target.OnDeath += OnDeath;
		_target.OnDamageTaken += OnDamageTaken;
	}

	private void OnDamageTaken()
	{
		var newLinePos = new Vector3();
		newLinePos = _heathLine.localPosition;
		newLinePos.x = 100 * (_target.HealthPercent - 1);
		_heathLine.localPosition = newLinePos;
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
