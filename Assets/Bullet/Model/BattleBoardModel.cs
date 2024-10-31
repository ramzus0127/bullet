using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BulletMaster;
using static CardMaster;
using static FieldManager;
using static UnitMaster;

[Serializable]
public class BattleBoardModel
{
	int _UnitCnt;
	public int _unitCnt { get => _UnitCnt; set => _UnitCnt = value; }

	int _BullCnt;
	public int _bullCnt { get => _BullCnt; set => _BullCnt = value; }

	[SerializeField]
	BattlePlayerModel _P1;
	public BattlePlayerModel _p1 { get => _P1; set => _P1 = value; }

	[SerializeField]
	BattlePlayerModel _P2;
	public BattlePlayerModel _p2 { get => _P2; set => _P2 = value; }

	Dictionary<int, UnitModel> _UnitDic;
	Dictionary<int, BulletModel> _BullDic;

	public void Init() 
	{
		_UnitCnt = 0;
		_BullCnt = 0;
		_P1.Init();
		_P2.Init();
		_UnitDic = new Dictionary<int, UnitModel>();
		_BullDic = new Dictionary<int, BulletModel>();
	}

	public UnitModel AddUnit(UnitTyp aTyp, Vector2 aPos, bool aIsP1)
	{
		var id = _UnitCnt;
		var unit = new UnitModel(id, aTyp, aPos, aIsP1);
		if (aIsP1)
			_p1._units.Add(unit);
		else 
			_p2._units.Add(unit);
		_UnitCnt++;
		return unit;
	}

	public BulletModel AddBull(BulletTyp aTyp, Vector2 aPos, Dir aDir, bool aIsP1)
	{
		var id = _BullCnt;
		var bull = new BulletModel(id, aTyp, aPos, aDir);
		if (aIsP1)
			_p1._bulls.Add(bull);
		else
			_p2._bulls.Add(bull);
		_BullCnt++;
		return bull;
	}

	public UnitModel GetUnit(int aId)
	{
		if (_UnitDic.ContainsKey(aId)) return _UnitDic[aId];

		var units = _p1._units;
		var unit = units.FirstOrDefault(a => a._id == aId);
		if (unit != null)
		{
			_UnitDic.Add(aId, unit);
			return _UnitDic[aId];
		}

		units = _p2._units;
		unit = units.FirstOrDefault(a => a._id == aId);
		if (unit != null)
		{
			_UnitDic.Add(aId, unit);
			return _UnitDic[aId];
		}
		return null;
	}

	public BulletModel GetBull(int aId)
	{
		if (_BullDic.ContainsKey(aId)) return _BullDic[aId];

		var bulls = _p1._bulls;
		var bull = bulls.FirstOrDefault(a => a._id == aId);
		if (bull != null)
		{
			_BullDic.Add(aId, bull);
			return _BullDic[aId];
		}

		bulls = _p2._bulls;
		bull = bulls.FirstOrDefault(a => a._id == aId);
		if (bull != null)
		{
			_BullDic.Add(aId, bull);
			return _BullDic[aId];
		}
		return null;
	}

	public void ExecCard(int aCardId, int aTile, bool aIsP1, Action<(bool isP1, UnitModel unit, FieldEffectModel fieldEffect)> aOnExec)
	{
		BattlePlayerModel player;
		if (aIsP1)
			player = _p1;
		else
			player = _p2;

		var card = player._deck.ExecCard(aCardId);
		player.UseEnergy(card._cost);

		if (card._category == Category.Unit)
		{
			var rent = ResourceManager.RentUnitPlacer(card._typ);
			var placers = rent.GetComponentsInChildren<UnitPlacer>();
			var pos = GetTilePos(aTile);
			rent.transform.position = pos;

			var ang = rent.transform.localEulerAngles;
			if (aIsP1)
				ang.y = 0f;
			else
				ang.y = 180f;
			rent.transform.localEulerAngles = ang;

			for (int i = 0; i < placers.Length; i++)
			{
				var placer = placers[i];
				placer.Exec(this, aIsP1, aOnExec);
			}
			rent.Return();
		}
	}

	public void SetupTowerAndTurret()
	{
		Vector2 pos;
		pos = GetTowerPos(TowerPos.P1Tower);	AddUnit(UnitTyp.Tower,	pos, true);
		pos = GetTowerPos(TowerPos.P1TurretL);	AddUnit(UnitTyp.Turret,	pos, true);
		pos = GetTowerPos(TowerPos.P1TurretR);	AddUnit(UnitTyp.Turret,	pos, true);
		pos = GetTowerPos(TowerPos.P2Tower);	AddUnit(UnitTyp.Tower,	pos, false);
		pos = GetTowerPos(TowerPos.P2TurretL);	AddUnit(UnitTyp.Turret,	pos, false);
		pos = GetTowerPos(TowerPos.P2TurretR);	AddUnit(UnitTyp.Turret,	pos, false);
	}

	public BattleBoardModel Copy() 
	{
		var copy = new BattleBoardModel();

		copy._UnitCnt = _UnitCnt;
		copy._BullCnt = _BullCnt;

		copy._P1 = _P1.Copy();
		copy._P2 = _P2.Copy();

		copy._UnitDic = new Dictionary<int, UnitModel>();
		copy._BullDic = new Dictionary<int, BulletModel>();

		return copy;
	}
}