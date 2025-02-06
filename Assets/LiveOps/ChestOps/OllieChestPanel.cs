using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class OllieChestPanel : MonoBehaviour
{
	private const string FloatId = "ollie_chest_float";
	private const string MainID = "ollie_chest_main";
	
	[SerializeField] private Button _tapToOpenButton;
	[SerializeField] private Transform _tapToOpenText;
	[SerializeField] private Image _chestIcon;
	[SerializeField] private Transform _chestHolder;
	[SerializeField] private Transform _shine;
	[SerializeField] private Transform _glow;
	[SerializeField] private Transform _beam;
	[SerializeField] private Transform _beam2;
	[SerializeField] private Image _chestGlowImage;
	[SerializeField] private Transform _slotPrefab;
	[SerializeField] private Transform _horizontal;
	[SerializeField] private Transform _horizontal2;
	[SerializeField] private OllieChestRewardView _rewardViewPrefab;
	[SerializeField] private Transform _rewardHolder;
	[SerializeField] private Material _chestRewardMaterial;
	[SerializeField] private Button _claimButton;
	[SerializeField] private CanvasGroup _claimCanvasGroup;
	[SerializeField] private Sprite _openedSprite;
	[SerializeField] private Sprite _closedSprite;
	[SerializeField] private CanvasGroup _ownCanvasGroup;

	[Header("Animation Feel")]
	[SerializeField] private float _floatDistance;
	[SerializeField] private float _floatDuration;
	[SerializeField] private AnimationCurve _floatEase;
	[SerializeField] private Ease _entireFloatEase;
	[SerializeField] private float _downDistance;
	[SerializeField] private Ease _downEase;
	[SerializeField] private float _downDuration;
	[SerializeField] private float _chestTargetScale;
	[SerializeField] private float _chestScaleDuration;
	[SerializeField] private Ease _chestScaleEase;
	[SerializeField] private float _chestDescaleDuration;
	[SerializeField] private Ease _chestDescaleEase;
	[SerializeField] private Ease _entireChestScaleEase;

	[SerializeField] private float _shineTargetScale;
	[SerializeField] private Ease _shineEase;
	[SerializeField] private float _beamTargetScale;
	[SerializeField] private Ease _beamEase;
	[SerializeField] private float _beam2TargetScale;
	[SerializeField] private Ease _beam2Ease;

	[SerializeField] private float _targetChestShineAlpha;
	[SerializeField] private float _chestShineAlphaDuration;
	[SerializeField] private Ease _chestShineEase;

	[SerializeField] private float _rewardJumpPower;
	[SerializeField] private float _rewardJumpDuration;
	[SerializeField] private Ease _rewardJumpEase;
	[SerializeField] private Ease _rewardScaleEase;
	[SerializeField] private Ease _rewardMoveEase;
	[SerializeField] private float _rewardMoveInterval;
	[SerializeField] private float _materialAppearDuration;
	[SerializeField] private Ease _materialEase;
	[SerializeField] private Ease _textEase;

	[SerializeField] private float _deshineDuration;
	[SerializeField] private Ease _deshineEase;

	private Action _currentClaimAction;
	[SerializeField] private RewardModel[] _rewardsToGrant;

	private Vector3 _originalPosition;

	private List<OllieChestRewardView> _chestRewards = new();
	private List<Transform> _slots = new();

	private void Awake()
	{
		_tapToOpenButton.onClick.AddListener(OnTapToOpenClick);
		//_originalPosition = _chestHolder.transform.localPosition;
		_claimButton.onClick.AddListener(OnClaimClick);
		_ownCanvasGroup.blocksRaycasts = false;
	}

	private void OnClaimClick()
	{
		_ownCanvasGroup.blocksRaycasts = false;
		_currentClaimAction?.Invoke();
		_ownCanvasGroup.DOFade(0f, 0.3f);
	}

	private void OnTapToOpenClick()
	{
		_tapToOpenButton.gameObject.SetActive(false);
		_tapToOpenText.gameObject.SetActive(false);

		DOTween.Kill(FloatId);

		var seq = DOTween.Sequence();
		seq.Append(_chestHolder.DOLocalMoveY(_originalPosition.y + _downDistance, _downDuration).SetEase(_downEase));

		var scaleSeq = DOTween.Sequence();
		scaleSeq.Append(_chestHolder.DOScale(_chestTargetScale, _chestScaleDuration).SetEase(_chestScaleEase));
		scaleSeq.AppendCallback(() =>
		{
			_chestIcon.sprite = _openedSprite;
			_chestIcon.SetNativeSize();
		});
		scaleSeq.Append(_chestHolder.DOScale(1f, _chestDescaleDuration).SetEase(_chestDescaleEase));
		scaleSeq.SetEase(_entireChestScaleEase);
		seq.Append(scaleSeq);

		seq.Append(_chestGlowImage.DOFade(_targetChestShineAlpha, _chestShineAlphaDuration).SetEase(_chestShineEase));
		seq.Join(_shine.DOScale(_shineTargetScale, _chestShineAlphaDuration).SetEase(_shineEase));
		seq.Join(_beam.DOScale(_beamTargetScale, _chestShineAlphaDuration).SetEase(_beamEase));
		seq.Join(_beam2.DOScale(_beam2TargetScale, _chestShineAlphaDuration).SetEase(_beam2Ease));

		var amountOfRewardsJumped = 0;
		
		foreach (var reward in _chestRewards)
		{
			var slotParent = _slots.Count >= 4 ? _horizontal2 : _horizontal;
			
			if (_slots.Count == 4)
			{
				_slots[_slots.Count - 1].SetParent(_horizontal2);
			}
			
			var slot = Instantiate(_slotPrefab, slotParent);
			_slots.Add(slot);

			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_horizontal.parent);

			seq.Append(reward.transform.DOJump(slot.transform.position, _rewardJumpPower, 1, _rewardJumpDuration).SetEase(_rewardJumpEase));
			seq.Join(reward.transform.DOScale(1f, _rewardJumpDuration).SetEase(_rewardScaleEase));
			for (var i = 0; i < amountOfRewardsJumped; i++)
			{
				seq.Join(_chestRewards[i].transform.DOMove(_slots[i].transform.position, _rewardJumpDuration).SetEase(_rewardMoveEase));
				seq.Join(_chestRewards[i].Glow.transform.DOScale(0f, _rewardJumpDuration).SetEase(Ease.Linear));
			}
			amountOfRewardsJumped++;
		}
		seq.AppendCallback(() =>
		{
			var shineSeq = DOTween.Sequence();
			shineSeq.Append(_chestGlowImage.DOFade(0f, _deshineDuration).SetEase(_deshineEase));
			shineSeq.Join(_shine.DOScale(0f, _deshineDuration).SetEase(_deshineEase));
			shineSeq.Join(_beam.DOScale(0f, _deshineDuration).SetEase(_deshineEase));
			shineSeq.Join(_beam2.DOScale(0f, _deshineDuration).SetEase(_deshineEase));
		});

		seq.Join(_chestRewards[_chestRewards.Count - 1].Glow.transform.DOScale(0f, _rewardJumpDuration).SetEase(Ease.Linear));

		seq.AppendCallback(() => _claimButton.gameObject.SetActive(true));

		seq.Append(_chestRewardMaterial.DOFloat(0f, "_Slider", _materialAppearDuration).SetEase(_materialEase));
		
		foreach (var view in _chestRewards)
		{
			seq.Join(view.Text.transform.DOScale(1f, _materialAppearDuration).SetEase(_textEase));
		}
		
		seq.Join(_claimCanvasGroup.DOFade(1f, _materialAppearDuration).SetEase(_materialEase));

		seq.SetEase(Ease.Linear);
		seq.SetId(MainID);
	}

	[Button]
	public void Open()
	{
		_ownCanvasGroup.blocksRaycasts = true;
		_ownCanvasGroup.DOFade(1f, 0.3f).SetDelay(2f);
		PlayAnimation();
	}

	private void PlayAnimation()
	{
		_tapToOpenButton.gameObject.SetActive(true);
		_tapToOpenText.gameObject.SetActive(true);
		_claimButton.gameObject.SetActive(false);
		_claimCanvasGroup.alpha = 0f;

		DOTween.Kill(FloatId);
		DOTween.Kill(MainID);

		foreach (var view in _chestRewards)
		{
			Destroy(view.gameObject);
		}
		
		_chestRewards.Clear();
		
		foreach (var slot in _slots)
		{
			Destroy(slot.gameObject);
		}
		
		_slots.Clear();

		_chestHolder.localPosition = _originalPosition;
		_shine.localScale = Vector3.zero;
		_beam.localScale = Vector3.zero;
		_beam2.localScale = Vector3.zero;
		_chestHolder.localScale = Vector3.one;
		var temp = _chestGlowImage.color;
		temp.a = 0f;
		_chestGlowImage.color = temp;
		_chestRewardMaterial.SetFloat("_Slider", 1f);

		_chestIcon.sprite = _closedSprite;

		var floatSeq = DOTween.Sequence();
		floatSeq.Append(_chestHolder.DOLocalMoveY(_originalPosition.y + _floatDistance, _floatDuration).SetEase(_floatEase));
		floatSeq.Append(_chestHolder.DOLocalMoveY(_originalPosition.y, _floatDuration).SetEase(_floatEase));
		floatSeq.SetEase(_entireFloatEase);
		floatSeq.SetLoops(-1, LoopType.Restart);
		floatSeq.SetId(FloatId);

		foreach (var reward in _rewardsToGrant)
		{
			var view = Instantiate(_rewardViewPrefab, _rewardHolder);
			view.transform.localPosition = Vector3.zero;
			view.Init(reward);
			_chestRewards.Add(view);
			view.Text.transform.localScale = Vector3.zero;
		}
	}
}