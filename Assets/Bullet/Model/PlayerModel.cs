using System;
using UnityEngine;

[Serializable]
public class PlayerModel
{
	[SerializeField]
	int _Id;
	public int _pId { get => _Id; set => _Id = value; }

	[SerializeField]
	int _Lv;
	public int _lv => _Lv;

	[SerializeField]
	string _Name;
	public string _name => _Name;

	[SerializeField]
	string _ClanName;
	public string _clanName => _ClanName;

	[SerializeField]
	int _RankPoint;
	public int _rankPoint => _RankPoint;

	[SerializeField]
	int _Coin;
	public int _coin => _Coin;

	[SerializeField]
	int _Gem;
	public int _gem => _Gem;

	public int _matchingRank100 => _RankPoint / 100;
	public int _matchingRank300 => _RankPoint / 300;
	public int _matchingRank900 => _RankPoint / 900;

	[SerializeField]
	CardModel[] _Cards;
	public CardModel[] _cards => _Cards;

	[SerializeField]
	DeckModel[] _Decks = new DeckModel[5];
	public DeckModel[] _decks { get => _Decks; set => _Decks = value; }

	public int _selectedDeckIdx { get; set; }

	public DeckModel _selectedDeck => _decks[_selectedDeckIdx];

	public void Init() 
	{
		for (int i = 0; i < _Cards.Length; i++) _Cards[i].SetId(i);
	}
}

