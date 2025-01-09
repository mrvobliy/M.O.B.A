using DG.Tweening;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
	[SerializeField] private Vector3 _barOffset;
	[SerializeField] private Vector3 _damageNumOffset;
	[SerializeField] private CanvasGroup _canvasGroup;
	[SerializeField] private RectTransform _heathLine;
	[SerializeField] private float _endPosValue = 100f;
	[SerializeField] private DamageNumber _damageNumber;

	private EntityHealthControl _entityHealthControl;
	private Tweener _canvasFadeAnim;

	public void Init(EntityHealthControl entityHealth)
	{
		_entityHealthControl = entityHealth;
		_entityHealthControl.OnDeathStart += OnDeath;
		_entityHealthControl.OnHealthChanged += OnHealthChanged;
		_entityHealthControl.OnEnemyAttackUs += TryShowHeathBar;
	}

	private void OnHealthChanged()
	{
		var newLinePos = new Vector3();
		newLinePos = _heathLine.localPosition;
		newLinePos.x = _endPosValue * (_entityHealthControl.HealthPercent - 1);
		_heathLine.localPosition = newLinePos;
	}

	private void OnDeath()
	{
		_canvasFadeAnim.Kill();
		
		_entityHealthControl.OnDeathStart -= OnDeath;
		_entityHealthControl.OnHealthChanged -= OnHealthChanged;
		_entityHealthControl.OnEnemyAttackUs -= TryShowHeathBar;
		
		Destroy(gameObject);
	}

	private void LateUpdate()
	{
		var screenPosition = Camera.main.WorldToScreenPoint(_entityHealthControl.transform.position + _barOffset);
		screenPosition.z = 0f;
		transform.position = screenPosition;
	}

	private void TryShowHeathBar(EntityComponentsData enemy, int damage)
	{
		if (enemy.transform.tag != "Player") return;

		_canvasFadeAnim = _canvasGroup.DOFade(1, 0.5f);
		CancelInvoke(nameof(HideHealthBar));
		Invoke(nameof(HideHealthBar), 10);
		
		var num = Instantiate(_damageNumber, _entityHealthControl.transform.position + _damageNumOffset, Quaternion.identity);
		num.SetDamageText(damage);
	}

	private void HideHealthBar() => _canvasFadeAnim = _canvasGroup.DOFade(0, 0.5f);
}
