using TMPro;
using UnityEngine;

public class CountDownView : MonoBehaviour
{
	[SerializeField]
	CurveAsset _CurveAsset;

    TextMeshProUGUI _TextCount;
    CanvasGroup _CanvasGroup;

	void Awake()
	{
		_TextCount = GetComponent<TextMeshProUGUI>();
		_CanvasGroup = GetComponent<CanvasGroup>();
	}

	public void UpdateView(int aFrame)
	{
		if (aFrame >= 60 * 4)
		{
			gameObject.SetActive(false);
		}
		else
		{
			gameObject.SetActive(true);

			var text = 3 - (aFrame / 60);
			if (text == 0)
				_TextCount.text = "Battle Start";
			else
				_TextCount.text = text.ToString();

			var lerp = (60 - aFrame % 60) / 60f;
			_TextCount.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 10, lerp);
			_CanvasGroup.alpha = Mathf.Lerp(1f, 0f, lerp);
		}
	}
}
