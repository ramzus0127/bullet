using UnityEngine.UI;

public class HPBarView : BattleViewBase
{
	int _Id = -1;

	Slider _Slider;
	UnitModel _unit => DataManager._battle._board.GetUnit(_Id);

	public override bool _remove => _unit._remove;

	void Awake()
	{
        _Slider = GetComponent<Slider>();
	}

	public void Init(UnitModel aUnit) 
	{
		_Id = aUnit._id;
		Init();
	}

	public override void UpdateView()
	{
		_Slider.value = _unit._hpBar;
		var pos3 = _unit._pos3;
		if (_unit._isP1)
			pos3.z -= 1.5f;
		else
			pos3.z += 1.5f;

		var pos = transform.parent.InverseTransformPoint(pos3);
		transform.localPosition = pos;
	}

	public override bool Return()
	{
		var ret = base.Return();
		if (ret) _Id = -1;
		return ret;
	}
}
