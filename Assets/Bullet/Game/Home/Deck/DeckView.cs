using UnityEngine;
using static ResourceManager;

public class DeckView : MonoBehaviour
{
	BoxCollider[] _BoxColliders;

	CardView[] _Views = new CardView[8];

	void Awake()
	{
		_BoxColliders = GetComponentsInChildren<BoxCollider>();
	}

	public void UpdateView(DeckModel aDeck) 
	{
		for (int i = 0; i < 8; i++)
		{
			var card = aDeck[i];
			var parent = _BoxColliders[i].transform;

			if (_Views[i]) 
			{
				if (card == null || !_Views[i]._card.Equals(card))
				{
					_Views[i].Return();
					_Views[i] = null;
				}
			}

			if (card == null) continue;

			if (!_Views[i])
			{
				var view = RentUI<CardView>(UIPrefabTyp.CardView);
				view.transform.SetParent(parent, false);
				view.transform.position = _BoxColliders[i].transform.position;
				view.InitHome(card);
				_Views[i] = view;
			}
		}
	}
}
