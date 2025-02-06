using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OllieChestRewardView : MonoBehaviour
{
	[SerializeField] private Image _image;
	[SerializeField] private TMP_Text _text;
	[SerializeField] private Transform _glow;

	public Image Image => _image;
	public TMP_Text Text => _text;
	public Transform Glow => _glow;

	public void Init(RewardModel reward)
	{
		_text.alpha = 0;
		_text.text = reward.Title;
		_image.sprite = reward.Sprite;
		_image.transform.localScale = reward.Scale * Vector3.one;
		transform.localScale = new Vector3(1, 1, 1);
		transform.localPosition = Vector3.zero;
	}
}