using UnityEngine;
using Toggle = UnityEngine.UI.Toggle;

public class Footer : MonoBehaviour
{
    Toggle[] _Toggles;

	public enum Mode 
	{
		Battle,
		Deck,
		Clan,
		Inventory,
		Shop,
		Steeing,
	}

	Mode _Mode;

	[SerializeField]
	HomeUIBase[] _HomeUIBases;

	void Awake()
	{
        _Toggles = GetComponentsInChildren<Toggle>();
	}

	void Start()
	{
		for (int i = 0; i < _Toggles.Length; i++)
		{
			var sel = i;

			if (_Mode == (Mode)i)
				_Toggles[i].SetIsOnWithoutNotify(true);

			_Toggles[i].onValueChanged.AddListener(res => OnTabSelected(res, sel));
		}

		Apply();
	}

	void OnTabSelected(bool aToggle, int aSelectedNum) 
	{
		if (aToggle)
		{ 
			_Mode = (Mode)aSelectedNum;
			Apply();
		}
	}

	void Apply()
	{
		for (int i = 0; i < _HomeUIBases.Length; i++)
		{
			if ((Mode)i == _Mode)
				_HomeUIBases[i].Show();
			else
				_HomeUIBases[i].Hide();
		}
	}
}
