using System;
using System.Collections.Generic;
using UnityEngine;
using static UnitMaster;

public class UnitModel
{
	UnitModel() { }

	public UnitModel(int aId, UnitTyp aTyp, Vector2 aPos, bool aIsP1)
	{
		_Id = aId;
		_Typ = aTyp;
		_IsP1 = aIsP1;
		_Pos = aPos;
		_Frame = 0;
		_HP = _master._hp;
		_HitMove = Vector2.zero;

		if (aIsP1)
		{
			_FaceDir = new Dir(0f);
			_MoveDir = new Dir(0f);
		}
		else
		{
			_FaceDir = new Dir(180f);
			_MoveDir = new Dir(180f);
		}

		_Guns = new List<GunModel>();

		for (int i = 0; i < _master._guns.Length; i++)
		{
			var gun = _master._guns[i];
			_Guns.Add(new GunModel(gun));
		}
	}

	int _Id;
	public int _id => _Id;

	UnitTyp _Typ;
	public UnitTyp _typ => _Typ;

	bool _IsP1;
	public bool _isP1 => _IsP1;

	Vector2 _Pos;
	public Vector3 _pos3 => new Vector3(_Pos.x, 0f, _Pos.y);
	public Vector2 _pos2 => _Pos;

	int _Frame;

	int _HP;
	public int _hp => _HP;
	public float _hpBar => (float)_HP / _master._hp;

	Vector2 _TageUnitOffset;
	public Vector3 _tageUnitPos => new Vector3(_Pos.x + _TageUnitOffset.x, 0f, _Pos.y + _TageUnitOffset.y);

	Dir _MoveDir;

	Dir _FaceDir;

	Vector2 _HitMove;

	List<GunModel> _Guns;

	public UnitMaster _master => MasterManager.GetUnitMaster(_Typ);
	public float _angle => _FaceDir._angle;
	public bool _hasUnitTage => _TageUnitOffset != default;
	float _searchRange => _master._searchRange;
	public Coll _coll => _master._coll;
	public bool _remove => _HP <= 0;
	public int _def => _master._def;
	public bool _isFixed => _master._moveTyp == MoveTyp.Fixed;			

	public void Act(BattleBoardModel aBoard,  Action<BulletModel> aOnFire)
	{
		switch (_master._turnTyp)
		{
			case TurnTyp.Rotate:
				_FaceDir.AddDir(_master._turnSpd);
				break;
			case TurnTyp.TurnToTage:
				if (_hasUnitTage)
				{
					_FaceDir.MoveTowards(_TageUnitOffset, _master._turnSpd);
				}
				else
				{
					if (_IsP1)
						_FaceDir.MoveTowards(0f, _master._turnSpd);
					else
						_FaceDir.MoveTowards(180f, _master._turnSpd);
				}
				break;
			case TurnTyp.Fixed:
				if (_IsP1)
					_FaceDir.SetDir(0f);
				else
					_FaceDir.SetDir(180f);
				break;
		}

		switch (_master._moveTyp)
		{
			case MoveTyp.Straight:
				if (_IsP1)
					_MoveDir.SetDir(0f);
				else
					_MoveDir.SetDir(180f);
				_Pos += _MoveDir._v2 * new Vector2(_master._spdXRatio, 1f) * _master._spd;
				break;
			case MoveTyp.SearchEne:
				if (_hasUnitTage)
				{
					if (_hasUnitTage)
						_MoveDir.SetDir(_TageUnitOffset);
				}
				else
				{
					if (_IsP1)
						_MoveDir.SetDir(0f);
					else
						_MoveDir.SetDir(180f);
				}
				_Pos += _MoveDir._v2 * new Vector2(_master._spdXRatio, 1f) * _master._spd;
				break;
			case MoveTyp.SearchAlly:
				break;
			case MoveTyp.Fixed:
				break;
		}

		switch (_master._fireTyp)
		{
			case FireTyp.Always:
				FireBull(aBoard, aOnFire);
				break;
			case FireTyp.HasTage:
				if (_hasUnitTage)
					FireBull(aBoard, aOnFire);
				break;
			case FireTyp.Never:
				break;
		}

		if (_master._moveTyp != MoveTyp.Fixed)
			_Pos += _HitMove;

		_HitMove = Vector2.zero;

		_Frame++;
	}

	void FireBull(BattleBoardModel aBoard, Action<BulletModel> aOnFire) 
	{
		for (int i = 0; i < _Guns.Count; i++)
		{
			var gun = _Guns[i];
			if (gun.IsFire())
			{
				var pos = _Pos + gun._offset.Rotate(_FaceDir._angle);
				var dir = _FaceDir + gun._dir;
				var bull = aBoard.AddBull(gun._typ, pos, dir, _IsP1);
				aOnFire(bull);
			}
		}
	}

	public bool SearchEne(bool aIsP1, List<UnitModel> aEnes)
	{
		_TageUnitOffset = default;

		if (_master._moveTyp == MoveTyp.SearchAlly) return false;

		UnitModel found = null;
		float foundDist = -1;

		for (int i = 0; i < aEnes.Count; i++)
		{
			var ene = aEnes[i];

			if (aIsP1)
			{
				if (ene._Pos.y < _Pos.y)
					continue;
			}
			else
			{ 
				if (ene._Pos.y > _Pos.y)
					continue;
			}
			
			float dist = (_pos3 - ene._pos3).sqrMagnitude;
			if (dist <= _searchRange * _searchRange)
			{
				if (found != null)
				{
					if (foundDist > dist)
					{
						found = ene;
						foundDist = dist;
					}
				}
				else
				{
					found = ene;
					foundDist = dist;
				}
			}
		}

		if (found == null)
		{
			return false;
		}
		else
		{ 
			_TageUnitOffset = found._Pos - _Pos;
			return true;
		}
	}

	public void HitUnit(UnitModel aTage) 
	{
		if (_master._moveTyp == MoveTyp.Fixed) return;

		var hit = _coll.IsHit(aTage._coll, aTage._pos2, _pos2);
		if (hit)
		{
			if (aTage._isFixed)
			{
				if (aTage._pos2.x < _pos2.x)
					_HitMove += Vector2.right * 0.1f;
				else
					_HitMove += Vector2.left * 0.1f;
			}
			else
			{ 
				_HitMove += (_pos2 - aTage._pos2).normalized * 0.1f;
			}
		}
	}

	public bool HitBull(BulletModel aBull) 
	{
		var hit = _coll.IsHit(aBull._pos2, _Pos);
		if (hit)
			_HP -= aBull._master._pow - _def;
		return hit;
	}

	public UnitModel Copy() 
	{
		var copy = new UnitModel();
		copy._Id = _Id;
		copy._Typ = _Typ;
		copy._IsP1 = _IsP1;
		copy._Pos = _Pos;
		copy._Frame = _Frame;
		copy._HP = _HP;
		copy._TageUnitOffset = _TageUnitOffset;
		copy._MoveDir = _MoveDir;
		copy._FaceDir = _FaceDir;
		copy._HitMove = _HitMove;

		copy._Guns = new List<GunModel>();

		for (int i = 0; i < _Guns.Count; i++)
		{
			var gun = _Guns[i];
			copy._Guns.Add(gun.Copy());
		}

		return copy;
	}
}

