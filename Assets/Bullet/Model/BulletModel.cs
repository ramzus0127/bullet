using System.Collections.Generic;
using UnityEngine;
using static BulletMaster;
using static ResourceManager;

public class BulletModel
{
	BulletModel() { }

	public BulletModel(int aId, BulletTyp aType, Vector2 aPos, Dir aDir)
	{
		_Id = aId;
		_Typ = aType;
		_Pos = aPos;
		_Frame = 0;
		_Dir = aDir;
	}

	int _Id;
	public int _id => _Id;

	BulletTyp _Typ;
	public BulletTyp _typ => _Typ;
	public BulletPrefabTyp _prefabTyp => _master._prefabTyp;

	Vector2 _Pos;
	public Vector3 _pos3 => new Vector3(_Pos.x, 0f, _Pos.y);

	public Vector2 _pos2 => _Pos;

	int _Frame;
	public int _frame => _Frame;

	Dir _Dir;

	public BulletMaster _master => MasterManager.GetBullMaster(_Typ);
	public bool _remove => _master._frame <= _Frame;
	public float _angle => _Dir._angle;

	public void Act()
	{
		_Pos += _Dir._v2 * _master._spd;
		_Frame++;
	}

	public void HitEnemy(List<UnitModel> aEnemys)
	{
		if (_remove) return;

		for (int i = 0; i < aEnemys.Count; i++) 
		{
			var ene = aEnemys[i];
			if (ene.HitBull(this))
			{ 
				_Frame = 999;
				break;
			}
		}
	}

	public BulletModel Copy() 
	{
		var copy = new BulletModel();
		copy._Id = _Id;
		copy._Typ = _Typ;
		copy._Pos = _Pos;
		copy._Frame	= _Frame;
		copy._Dir = _Dir;	
		return copy;
	}
}


