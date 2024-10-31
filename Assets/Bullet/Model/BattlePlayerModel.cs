using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BattlePlayerModel
{
	BattlePlayerModel() { }

	public BattlePlayerModel(PlayerModel aPlayer)
	{
		_pId = aPlayer._pId;
		_Deck = new BattleDeckModel(aPlayer._selectedDeck._cards);
	}

	[SerializeField]
	int _PId;
	public int _pId { get => _PId; set => _PId = value; }

	[SerializeField]
	BattleDeckModel _Deck;
	public BattleDeckModel _deck => _Deck;

	List<UnitModel> _Units;
	public List<UnitModel> _units => _Units;

	List<BulletModel> _Bulls;
	public List<BulletModel> _bulls => _Bulls;

	int _EnergyTotal;

	int _EnergyUsed;

	int _EnergyOverflowed;

	public bool _isP1 => _pId == DataManager._p1Id;
	public int _energy => _EnergyTotal - _EnergyUsed - _EnergyOverflowed;
	public float _energyGauge => _energy / 60f;

	public void Init() 
	{
		_Units = new List<UnitModel>();
		_Bulls = new List<BulletModel>();
		_Deck.Init();
	}

	public int GetPlayerNum(BattleModel aBattle)
	{
		if (aBattle._p1._pId == _pId)
			return 1;
		else
			return 2;
	}

	public void IncEnergy()
	{
		_EnergyTotal++;
		if (_energy > 600)
			_EnergyOverflowed += _energy - 600;
	}

	public void UseEnergy(int aCost)
	{
		_EnergyUsed += aCost * 60;
	}

	public void UnitAct(BattleBoardModel aBoard, Action<BulletModel> aOnFire)
	{
		for (int i = 0; i < _Units.Count; i++)
		{
			_Units[i].Act(aBoard, aOnFire);
		}

		_Units.RemoveAll(a => a._remove);
	}

	public void UnitSearch(bool aIsP1, List<UnitModel> aEnes)
	{
		for (int i = 0; i < _Units.Count; i++)
		{
			var unit = _Units[i];
			unit.SearchEne(aIsP1, aEnes);
		}
	}

	public void UnitHitUnit(List<UnitModel> aEnes)
	{
		for (int i = 0; i < _Units.Count; i++)
		{
			var unit = _Units[i];

			for (int ii = 0; ii < _Units.Count; ii++)
			{
				var unit2 = _Units[ii];
				if (unit == unit2) continue;
				unit.HitUnit(unit2);
			}

			for (int ii = 0; ii < aEnes.Count; ii++)
			{
				var unit2 = aEnes[ii];
				unit.HitUnit(unit2);
			}
		}
	}

	public void BullAct()
	{
		for (int i = 0; i < _Bulls.Count; i++)
		{
			_Bulls[i].Act();
		}
		_Bulls.RemoveAll(a => a._remove);
	}

	public void BullHitEne(List<UnitModel> aEnes)
	{
		for (int i = 0; i < _Bulls.Count; i++)
		{
			_Bulls[i].HitEnemy(aEnes);
		}
	}

	public BattlePlayerModel Copy() 
	{
		var copy = new BattlePlayerModel();
		copy._PId = _PId;
		copy._Deck = _Deck.Copy();
		copy._Units = new List<UnitModel>();
		copy._Bulls = new List<BulletModel>();

		for (int i = 0; i < _Units.Count; i++)
		{
			var unit = _Units[i];
			copy._Units.Add(unit.Copy());
		}

		for (int i = 0; i < _Bulls.Count; i++)
		{
			var bullet = _Bulls[i];
			copy._Bulls.Add(bullet.Copy());
		}

		copy._EnergyTotal = _EnergyTotal;
		copy._EnergyUsed = _EnergyUsed;
		copy._EnergyOverflowed = _EnergyOverflowed;

		return copy;
	}
}

