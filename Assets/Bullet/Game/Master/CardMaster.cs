using System;
using UnityEngine;
using static UnitMaster;

[Serializable]
public class CardMaster
{
	public enum Category
	{
		Unit,
		FieldEffect,
		Building,
	}

	public enum CardTyp
	{
		Finger,
		FingerX2,
		Wall,
		Wing,
		WingFingerX2,
		Decoy,
		Missile,
		Magnet,
	}

	[SerializeField]
	CardTyp _Typ;
	public CardTyp _typ => _Typ;
	public void SetTyp(int aIdx) => _Typ = (CardTyp)aIdx;

	[SerializeField]
	Category _Category;
	public Category _category => _Category;

	[SerializeField]
	int _Cost;
	public int _cost => _Cost;

	public string _name => _typ.ToString();
	
	public UnitTyp? _unitTyp => _Category == Category.Unit ? (UnitTyp)_typ : null;

	public UnitMaster _unitMaster => _unitTyp.HasValue ? MasterManager.GetUnitMaster(_unitTyp.Value) : null;
}

