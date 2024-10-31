using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DeckModel
{
	public DeckModel()
	{
		_CardIds = new List<int>();

		for (int i = 0; i < 8; i++)
		{
			_CardIds.Add(-1);
		}
	}

	[SerializeField]
	List<int> _CardIds;

	public CardModel this[int i] 
	{
		get 
		{
			if (_CardIds[i] == -1) return null;
			var id = _CardIds[i];
			return DataManager._player._cards[id];
		}

		set => _CardIds[i] = value._id;
	} 

	public List<CardModel> _cards 
	{
		get 
		{
			var cardModels = new List<CardModel>();
			for (int i = 0; i < 8; i++)
			{
				cardModels.Add(this[i]);
			}
			return cardModels;
		}
	}

	public bool _isReady => !_CardIds.Contains(-1);
}

