using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour
{
	[SerializeField]
	Transform _3DModelRoot;

	[SerializeField]
	GameObject _ActiveRoot;

	[SerializeField]
	TextMeshProUGUI _TextName;

	[SerializeField]
	TextMeshProUGUI _TextCost;

	[SerializeField]
	TextMeshProUGUI _TextLv;

	[SerializeField]
	TextMeshProUGUI _TextLvUpRequired;

	[SerializeField]
	Transform _LinkTarget;

	LinkTransform _LinkTransform;

	Canvas _Canvas;

	Transform _TraCam;

	public CardModel _card { get; private set; }

	Rent _Rent;
	Rent _Rent3DModel;
	Drag _Drag;
	Rent _DragRent3DModel;

	string _Tile; 

	public bool _isDrag { get; private set; }

	Transform _OriParent;

	void Awake()
	{
		_TraCam = Camera.main.transform;
		_LinkTransform = GetComponentInChildren<LinkTransform>();
		enabled = false;
	}

	public void Return() 
	{
		_LinkTransform.Cancel();
		_Rent3DModel.Return();
		_Rent.Return();

		_Rent3DModel = null;
		_Rent = null;
	}

	public void UpdateView(CardModel aCard)
	{
		InitSub(aCard);
	}

	void InitSub(CardModel aCard)
	{
		_Rent = GetComponent<Rent>();
		_card = aCard;

		_Rent3DModel = ResourceManager.RentCard3DModel(aCard._typ);
		_Rent3DModel.transform.SetParent(_3DModelRoot, false);
		_Rent3DModel.transform.localPosition = Vector3.zero;
		_Rent3DModel.transform.localEulerAngles = Vector3.zero;
		_Rent3DModel.transform.localScale = Vector3.one;

		_Canvas = _Rent3DModel.GetComponentInParent<Canvas>();
		var layerName = LayerMask.LayerToName(_Canvas.gameObject.layer);

		layerName = layerName.Replace("UI", "3DUI");
		_Rent3DModel.transform.SetLayer(layerName);

		_TextName.text = aCard._name;
		_TextCost.text = aCard._cost.ToString();
		_TextLv.text = $"Lv{aCard._lv}";

		SetLinkTransform(_Canvas);
	}

	public void InitHome(CardModel aCard) 
	{
		InitSub(aCard);
	}

	public void InitBattle(CardModel aCard) 
	{
		InitSub(aCard);
		enabled = true;
		_TextLvUpRequired.gameObject.SetActive(false);
		InitBattleDrag();
	}

	void SetLinkTransform(Canvas aCanvas) 
	{
		var parent = aCanvas.transform.parent.Find($"{aCanvas.name}2");
		_LinkTransform.Link(_LinkTarget, parent, _LinkTarget);
	}

	void InitBattleDrag() 
	{
		_Drag = gameObject.GetOrAddComponent<Drag>();
		_Drag.Init(false,
			() => 
			{
				_isDrag = true;
				_OriParent = transform.parent;
				transform.SetParent(BattleMain._dragRoot);

				_Canvas = _Rent3DModel.GetComponentInParent<Canvas>();
				Set3DModelLayer();
				SetLinkTransform(_Canvas);
			},
			() =>
			{
				_isDrag = false;
				transform.SetParent(_OriParent);
				transform.localPosition = Vector3.zero;

				_Canvas = _Rent3DModel.GetComponentInParent<Canvas>();
				Set3DModelLayer();
				SetLinkTransform(_Canvas);
			});
	}

	void Set3DModelLayer()
	{
		var canvas = _Rent3DModel.GetComponentInParent<Canvas>();
		var layerName = LayerMask.LayerToName(canvas.gameObject.layer);

		layerName = layerName.Replace("UI", "3DUI");
		_Rent3DModel.transform.SetLayer(layerName);
	}

	public void SetActive3DModel(bool aActive) 
	{
		_Rent3DModel.gameObject.SetActive(aActive);
	}

	public void SetEnableDrag(bool aEnable) 
	{
		if (_Drag)
			_Drag.enabled = aEnable;
	}

	void Update()
	{
		if (_card._wait) return;

		if (Input.GetMouseButtonUp(0))
		{
			if (_Tile != null)
				UseCard();
		}

		if (_card._wait)
		{
			_ActiveRoot.SetActive(false);
			_LinkTransform.gameObject.SetActive(false);
			return;
		}

		_ActiveRoot.SetActive(true);
		_LinkTransform.gameObject.SetActive(true);

		var dir = transform.position - _TraCam.position;
		if (Physics.Raycast(_TraCam.position, dir, out var hitInfo))
		{
			_LinkTransform.Cancel();
			_ActiveRoot.SetActive(false);

			if (!_DragRent3DModel)
			{ 
				_DragRent3DModel = ResourceManager.RentUnitPlacer(_card._typ);
				_DragRent3DModel.gameObject.SetActive(true);
				if (!DataManager._battle._isP1)
					_DragRent3DModel.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
				else
					_DragRent3DModel.transform.localEulerAngles = Vector3.zero;
			}
			_DragRent3DModel.transform.position = hitInfo.collider.transform.position;
			_Tile = hitInfo.collider.name;
		}
		else
		{
			if (_DragRent3DModel)
			{ 
				_DragRent3DModel.Return();
				_DragRent3DModel = null;
			}
			SetLinkTransform(_Canvas);
			_ActiveRoot.SetActive(true);
			_Tile = null;
		}
	}

	void UseCard()
	{
		if (_card._cost > DataManager._myBattlePlayer._energyGauge) 
			return;

		_card._wait = true;
		BattleConnectionManager.SendUseCard(short.Parse(_Tile), (byte)_card._id);
	}
}

