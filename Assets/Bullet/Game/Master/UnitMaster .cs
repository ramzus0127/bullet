using System;
using UnityEngine;
using static BulletMaster;
using static ResourceManager;

[Serializable]
public class UnitMaster
{
	public enum UnitTyp
	{
		Finger,
		Wall,
		Wing,
		Decoy,
		Tower,
		Turret,
	}

	[SerializeField]
	UnitTyp _Typ;
	public UnitTyp _typ => _Typ;
	public void SetTyp(int aIdx) => _Typ = (UnitTyp)aIdx;

	public enum MoveTyp 
	{
		Straight,
		SearchEne,
		SearchAlly,
		Fixed,
	}

	[SerializeField]
	MoveTyp _MoveTyp;
	public MoveTyp _moveTyp => _MoveTyp;

	public enum TurnTyp
	{
		Rotate,
		TurnToTage,
		Fixed,
	}

	[SerializeField]
	TurnTyp _TurnTyp;
	public TurnTyp _turnTyp => _TurnTyp;

	public enum FireTyp
	{
		Always,
		HasTage,
		Never,
	}

	[SerializeField]
	FireTyp _FireTyp;
	public FireTyp _fireTyp => _FireTyp;

	[SerializeField]
	int _HP;
	public int _hp => _HP;

	[SerializeField]
	int _Def;
	public int _def => _Def;

	[SerializeField]
	int _Pow;
	public int _pow => _Pow;

	[SerializeField]
	float _Spd;
	public float _spd => _Spd;

	[SerializeField]
	float _SpdXRatio = 1f;
	public float _spdXRatio => _SpdXRatio;

	[SerializeField]
	int _TurnSpd;
	public int _turnSpd => _TurnSpd;

	[SerializeField]
	int _SearchRangeTile;
	public float _searchRange => _SearchRangeTile * FieldManager._tileSize;

	[SerializeField]
	Coll _Coll;

	public Coll _coll => _Coll;

	[Serializable]
	public struct Gun
	{
		[SerializeField]
		Vector2 _Offset;
		public Vector2 _offset => _Offset;

		[SerializeField]
		BulletTyp _Typ;
		public BulletTyp _typ => _Typ;

		[SerializeField]
		Dir _Dir;
		public Dir _dir => _Dir;

		[SerializeField]
		int _IniWait;
		public int _iniWait => _IniWait;

		[SerializeField]
		int _Cnt;
		public int _cnt => _Cnt;

		[SerializeField]
		int _Inter;
		public int _inter => _Inter;

		[SerializeField]
		int _Wait;
		public int _wait => _Wait;
	}

	[SerializeField]
	Gun[] _Guns;

	public Gun[] _guns => _Guns;
}

