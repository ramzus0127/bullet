using UnityEngine;
using static BulletMaster;
using static UnitMaster;

public class GunModel
{
	public GunModel() { }

	public GunModel(Gun aGun) 
	{
		_Gun = aGun;
		_TotalFrame = _inter * _cnt + _wait;
	}

	Gun _Gun;
	int _TotalFrame; 
	int _CurFrame;
	int _CurCnt;

	int _frame => _CurFrame - _Gun._iniWait;
	int _inter => _Gun._inter;
	int _cnt => _Gun._cnt;
	int _wait => _Gun._wait;

	public BulletTyp _typ => _Gun._typ;
	public Vector2 _offset => _Gun._offset;
	public Dir _dir => _Gun._dir;

	public bool IsFire() 
	{
		bool ret = false;
		if (_frame == _TotalFrame)
		{ 
			_CurFrame -= _TotalFrame;
			_CurCnt = 0;
		}

		if (_frame >= 0 && _CurCnt < _cnt)
		{
			if (_frame % _inter == 0)
			{ 
				ret = true;
				_CurCnt++;
			}
		}

		_CurFrame++;

		return ret;
	}


	public GunModel Copy() 
	{
		var copy = new GunModel();
		copy._Gun = _Gun;
		copy._TotalFrame = _TotalFrame;
		copy._CurFrame = _CurFrame;
		copy._CurCnt = _CurCnt;
		return copy;
	}
}

