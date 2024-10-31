using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BattleDeckModel
{
	BattleDeckModel() { }

	public BattleDeckModel(List<CardModel> aCards)
	{
		_Cards = aCards;
	}

	[SerializeField]
	List<CardModel> _Cards;

	List<int> _CardIds;
	public List<int> _cardIds => _CardIds;

	public void Init()
	{
		_CardIds = new List<int>();
		for (int i = 0; i < _Cards.Count; i++)
		{ 
			_Cards[i].SetId(i);
			_CardIds.Add(i);
		}
	}

	public CardModel GetCard(int aCardId) => _Cards[aCardId];

	public CardModel ExecCard(int aCardId)
	{
		var useIdx = _CardIds.IndexOf(aCardId);

		_CardIds[useIdx] = _CardIds[4];
		_CardIds[4] = _CardIds[5];
		_CardIds[5] = _CardIds[6];
		_CardIds[6] = _CardIds[7];
		_CardIds[7] = aCardId;

		_Cards[aCardId]._wait = false;
		return _Cards[aCardId];
	}

	public BattleDeckModel Copy() 
	{ 
		var copy = new BattleDeckModel();

		copy._Cards = new List<CardModel>();
		for (int i = 0; i < _Cards.Count; i++)
		{
			var card = _Cards[i];
			copy._Cards.Add(card.Copy());
		}

		copy._CardIds = new List<int>();
		for (int i = 0; i < _CardIds.Count; i++)
		{
			copy._CardIds.Add(_CardIds[i]);
		}

		return copy;
	}
}

