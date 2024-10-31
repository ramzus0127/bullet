using System;
using UnityEngine;
using static CardMaster;
using static UnitMaster;

[Serializable]
public class CardModel : IEquatable<CardModel> 
{
	protected int _Id;
	public int _id => _Id;
	public void SetId(int aIdx) => _Id = aIdx;

	[SerializeField]
	protected CardTyp _Typ;

	[SerializeField]
	protected int _Lv;
	public int _lv => _Lv;

	public CardMaster _master => MasterManager.GetCardMaster(_Typ);
	public CardTyp _typ => _master._typ;
	public Category _category => _master._category;
	public string _name => _master._name;
	public int _cost => _master._cost;

	public bool _wait { get; set; }

	public bool Equals(CardModel other)
	{
		return _Typ == other._Typ;
	}

	public CardModel Copy()
	{
		var copy = new CardModel();

		copy._Id = _Id;
		copy._Typ = _Typ;
		copy._Lv = _Lv;

		return copy;
	}
}

