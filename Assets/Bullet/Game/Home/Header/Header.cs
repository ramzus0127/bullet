using TMPro;
using UnityEngine;

public class Header : MonoBehaviour
{
	[SerializeField]
	TextMeshProUGUI _NameText;

	[SerializeField]
	TextMeshProUGUI _ClanNameText;

	[SerializeField]
    TextMeshProUGUI _LvText;

    void Start()
    {
        _NameText.text = DataManager._player._pId.ToString();
		_ClanNameText.text = DataManager._player._clanName;
		_LvText.text = $"Lv{DataManager._player._lv}";
	}
}
