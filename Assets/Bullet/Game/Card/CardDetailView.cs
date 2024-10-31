using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDetailView : MonoBehaviour
{
	static int gId;

	[SerializeField]
	Transform _3DModelRoot;

	[SerializeField]
	TextMeshProUGUI _TxtName;

	[SerializeField]
	TextMeshProUGUI _TxtCost;

	[SerializeField]
	TextMeshProUGUI _TxtLv;

	Rent _Rent;
	Rent _Rent3DModel;

	Button _BtnClose;

	TweenPosition _TweenPos;

	public TweenPosition _tweenPos => _TweenPos;

	Transform _CloseRoot;

	Action<int> _OnCloseDone;

	public int _id { get; private set; }

	void Awake()
	{
		_BtnClose = GetComponentInChildren<Button>();
		_TweenPos = GetComponent<TweenPosition>();
	}

	void Start()
	{
		_BtnClose.onClick.AddListener(Close);
	}

	void Return() 
	{
		_Rent3DModel.Return();
		_Rent.Return();

		_Rent3DModel = null;
		_Rent = null;

		_OnCloseDone(_id);
	}

	public async void Open(CardModel aCard, Transform aCloseRoot, Action<int> aOnCloseDone)
	{
		_id = gId;
		gId++;

		_OnCloseDone = aOnCloseDone;

		_Rent = GetComponent<Rent>();
		_TweenPos.SetAtStart();
		_CloseRoot = aCloseRoot;

		_Rent3DModel = ResourceManager.RentCard3DModel(aCard._typ);
		_Rent3DModel.transform.SetParent(_3DModelRoot, false);

		Set3DModelLayer();

		_TxtName.text = aCard._name;
		_TxtCost.text = aCard._cost.ToString();
		_TxtLv.text = $"Lv{aCard._lv}";

		await _TweenPos.PlayAsync();
	}

	public async void Close()
	{
		transform.SetParent(_CloseRoot, false);

		Set3DModelLayer();

		await _TweenPos.RewindAsync();
		Return();
	}

	void Set3DModelLayer() 
	{
		var canvas = _Rent3DModel.GetComponentInParent<Canvas>();
		var layerName = LayerMask.LayerToName(canvas.gameObject.layer);

		layerName = layerName.Replace("UI", "3DUI");
		_Rent3DModel.transform.SetLayer(layerName);
	}
}

