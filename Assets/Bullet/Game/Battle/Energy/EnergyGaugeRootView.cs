using TMPro;
using UnityEngine;

public class EnergyGaugeRootView : MonoBehaviour
{
	[SerializeField]
	TextMeshProUGUI _TxtCurEnergy;

    EnergyGaugeView[] _EnergyGaugeViews;

	void Awake()
	{
		_EnergyGaugeViews = GetComponentsInChildren<EnergyGaugeView>();
	}

	public void UpdateView(float aGauge)
	{
		_TxtCurEnergy.text = $"{(int)aGauge}";

		for (int i = 0; i < _EnergyGaugeViews.Length; i++)
		{
			_EnergyGaugeViews[i].UpdateView(aGauge - i);
		}
	}
}
