using System;
using UnityEngine;
using static ResourceManager;
using static UnitMaster;

[Serializable]
public class BulletMaster
{
	public enum BulletTyp
	{
		LaserS,
		LaserM,
		LaserL,

		BeamS,
		BeamM,
		BeamL,
	}

	[SerializeField]
	BulletTyp _Typ;
	public BulletTyp _typ => _Typ;
	public void SetTyp(int aIdx) => _Typ = (BulletTyp)aIdx;

	[SerializeField]
	BulletPrefabTyp _PrefabTyp;
	public BulletPrefabTyp _prefabTyp => _PrefabTyp;

	[SerializeField]
	float _Spd;
	public float _spd => _Spd;

	[SerializeField]
	int _Pow;
	public int _pow => _Pow;

	[SerializeField]
	int _Frame;
	public int _frame => _Frame;
}