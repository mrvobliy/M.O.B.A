using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OllieChestRewardView : MonoBehaviour
{
	[SerializeField] private Image _image;
	[SerializeField] private TMP_Text _text;

	public void Init(RewardModel reward)
	{
		//_text.alpha = 0;
		_text.text = reward.Title;
		_image.sprite = reward.Sprite;
		
		//transform.localScale = new Vector3(1f, 1f, 1f);
		//transform.localPosition = Vector3.zero;
		//gameObject.SetActive(false);
	}
}