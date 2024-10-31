using System.Collections.Generic;
using UnityEngine;
using static ResourceManager;

public class CardRootView : MonoBehaviour
{
	[SerializeField]
	Transform[] _CardViewPosTras;

	List<CardView> _Views = new List<CardView>();

	BattleDeckModel _BattleDeck;

	public void Clear() 
	{
		for (int i = 0; i < _Views.Count; i++)
		{
			_Views[i].Return();
		}
		_Views.Clear();
	}

	public void Init(BattleDeckModel aBattleDeck) 
	{
		_BattleDeck = aBattleDeck;
		for (int i = 0; i < 8; i++)
		{
			var view = RentUI<CardView>(UIPrefabTyp.CardView);
			view.transform.SetParent(_CardViewPosTras[i], false);
			view.InitBattle(_BattleDeck.GetCard(i));
			_Views.Add(view);
		}
	}

	public void UpdateView() 
	{
		for (int i = 0; i < 8; i++)
		{
			var cardView = _Views[_BattleDeck._cardIds[i]];
			var tra = cardView.transform;

			if (!cardView._isDrag)
				tra.SetParent(_CardViewPosTras[i], false);
			cardView.SetActive3DModel(Get3DModelActive(tra));
			cardView.SetEnableDrag(GetDragEnable(tra));
		}
	}

	bool Get3DModelActive(Transform aTra)
	{
		if (aTra.parent.name == "7") return false;
		if (aTra.parent.name == "6") return false;
		if (aTra.parent.name == "5") return false;
		return true;
	}

	bool GetDragEnable(Transform aTra)
	{
		if (aTra.parent.name == "7") return false;
		if (aTra.parent.name == "6") return false;
		if (aTra.parent.name == "5") return false;
		if (aTra.parent.name == "4") return false;
		return true;
	}
}
