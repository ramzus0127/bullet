using UnityEngine;
using UnityEngine.UI;
using static ResourceManager;
using Toggle = UnityEngine.UI.Toggle;

public class DeckHomeUI : HomeUIBase
{
	Toggle[] _DeckSelectToggles;

	[SerializeField]
	DeckView _DeckView;

	[SerializeField]
	Transform _CardListViewRoot;

	[SerializeField]
	Transform _CardDetailViewRoot;

	[SerializeField]
	Transform _DragRoot;

	[SerializeField]
	GraphicRaycaster _GraphicRaycaster;

	[SerializeField]
	ScrollRect _ScrollRect;

	[SerializeField]
	Canvas _Canvas;

	[SerializeField]
	CardDetailRoot _CardDetailRoot;

	Transform _TraCam;

	void Awake()
	{
		_DeckSelectToggles = GetComponentsInChildren<Toggle>();
		_TraCam = Camera.main.transform;
	}

	void Start()
	{
		for (int i = 0; i < _DeckSelectToggles.Length; i++)
		{
			var sel = i;
			_DeckSelectToggles[i].onValueChanged.AddListener(res => OnDeckSelect(res, sel));
		}

		var cards = DataManager._player._cards;
		for (int i = 0; i < cards.Length; i++)
		{
			var card = cards[i];
			var view = RentUI<CardView>(UIPrefabTyp.CardView);
			view.transform.SetParent(_CardListViewRoot, false);
			view.InitHome(card);
			view.gameObject.AddComponent<LongTap>().Init(0.3f, 30f, () => OnStartDrag(card, view.transform));
			view.gameObject.AddComponent<ShortTap>().Init(0.2f, 30f, () => OnShortTap(card));
		}

		UpdateDeckView();
	}

	void OnShortTap(CardModel aCardModel) 
	{
		_CardDetailRoot.Open(aCardModel);
	}

	protected override void OnHide()
	{
		_CardDetailRoot.Close();
	}

	void OnStartDrag(CardModel aCard, Transform aTra) 
	{
		_GraphicRaycaster.enabled = false;
		_ScrollRect.enabled = false;

		var view = RentUI<CardView>(UIPrefabTyp.CardView);
		view.transform.SetParent(_DragRoot, false);
		view.transform.position = aTra.transform.position + new Vector3(1f, 1f, 0f);

		view.InitHome(aCard);
		var drag = view.gameObject.AddComponent<Drag>();

		drag.Init(true, null,
			() => 
			{
				_GraphicRaycaster.enabled = true;
				_ScrollRect.enabled = true;

				var cardPos = view.transform.position;
				var dir = cardPos - _TraCam.position;

				view.Return();
				DestroyImmediate(drag);

				if (Physics.Raycast(_TraCam.position, dir, out var hitInfo))
				{
					var idx = hitInfo.collider.transform.GetSiblingIndex();
					DeckModel seledtedDeckModel = DataManager._player._selectedDeck;
					seledtedDeckModel[idx] = view._card;
					DataManager.SavePlayerModel();
					UpdateDeckView();
				}
			});
	}

	void OnDeckSelect(bool aToggle, int aSelectedNum)
	{
		if (aToggle)
		{ 
			DataManager._player._selectedDeckIdx = aSelectedNum;
			UpdateDeckView();
		}
	}

	void UpdateDeckView() 
	{
		DeckModel seledtedDeckModel = DataManager._player._selectedDeck;
		_DeckView.UpdateView(seledtedDeckModel);
	}
}
