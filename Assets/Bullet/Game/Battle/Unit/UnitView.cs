using UnityEngine;

public class UnitView : BattleViewBase
{
	UnitModel _unit => DataManager._battle._board.GetUnit(_Id);
	LineRenderer _LR;

	int _Id = -1;

	public override bool _remove => _unit._remove;

	void Awake()
	{
		_LR = gameObject.GetComponent<LineRenderer>();
	}

	void Start()
	{
		if (_LR) _LR.enabled = false;
	}

	void OnDrawGizmos()
	{
		if (!Application.isPlaying) return;
		if (_unit == null) return;

		Gizmos.DrawWireCube(_unit._pos3, _unit._master._coll._size);
	}

	public void Init(UnitModel aUnit) 
	{
		_Id = aUnit._id;
		Init();
	}

	public override void UpdateView()
	{
		transform.position = _unit._pos3;
		if (_unit._hasUnitTage)
		{
			_LR.enabled = true;
			var pos = new Vector3[] { _unit._pos3, _unit._tageUnitPos };
			_LR.SetPositions(pos);
		}
		else
		{
			_LR.enabled = false;
		}

		var ang = transform.localEulerAngles;
		ang.y = _unit._angle;
		transform.localEulerAngles = ang;
	}

	public override bool Return()
	{
		var ret = base.Return();

		if (ret)
		{ 
			_LR.enabled = false;
			_Id = -1;
		}
		return ret;
	}
}
