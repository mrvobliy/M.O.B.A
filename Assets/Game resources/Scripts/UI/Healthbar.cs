using DG.Tweening;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
	[SerializeField] private Vector3 _offset;
	[SerializeField] private CanvasGroup _canvasGroup;
	[SerializeField] private RectTransform _heathLine;
	[SerializeField] private float _endPosValue = 100f;
	[SerializeField] private DamageNumber _damageNumber;

	private Target _target;
	private Tweener _canvasFadeAnim;

	public void Init(Target target)
	{
		_target = target;
		_target.OnDeath += OnDeath;
		_target.OnHealthChanged += OnHealthChanged;
		_target.OnEnemyAttackUs += TryShowHeathBar;
	}

	private void OnHealthChanged()
	{
		var newLinePos = new Vector3();
		newLinePos = _heathLine.localPosition;
		newLinePos.x = _endPosValue * (_target.HealthPercent - 1);
		_heathLine.localPosition = newLinePos;
	}

	private void OnDeath()
	{
		_canvasFadeAnim.Kill();
		
		_target.OnDeath -= OnDeath;
		_target.OnHealthChanged -= OnHealthChanged;
		_target.OnEnemyAttackUs -= TryShowHeathBar;
		
		Destroy(gameObject);
	}

	private void LateUpdate()
	{
		var position = _target.transform.position + _offset;
		var screenPosition = Camera.main.WorldToScreenPoint(position);
		screenPosition.z = 0f;

		transform.position = screenPosition;
	}

	private void TryShowHeathBar(Target enemy, int damage)
	{
		if (enemy.transform.tag != "Player") return;

		_canvasFadeAnim = _canvasGroup.DOFade(1, 0.5f);
		CancelInvoke(nameof(HideHealthBar));
		Invoke(nameof(HideHealthBar), 10);
		
		var num = Instantiate(_damageNumber, _target.transform.position, Quaternion.identity);
		num.SetDamageText(damage);
	}

	private void HideHealthBar()
	{
		_canvasFadeAnim = _canvasGroup.DOFade(0, 0.5f);
	}
}
