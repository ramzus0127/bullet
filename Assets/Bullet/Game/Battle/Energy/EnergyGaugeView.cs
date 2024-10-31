using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergyGaugeView : MonoBehaviour
{
    TextMeshProUGUI _TxtLabel;

	[SerializeField]
	Image _ImgGauge;

	void Awake()
	{
		_TxtLabel = GetComponentInChildren<TextMeshProUGUI>();
		_TxtLabel.text = gameObject.name;
	}

	public void UpdateView(float aGauge) 
	{
		_ImgGauge.fillAmount = aGauge;
	}
}
