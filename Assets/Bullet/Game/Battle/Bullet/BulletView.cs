public class BulletView : BattleViewBase
{
	BulletModel _bull => DataManager._battle._board.GetBull(_Id);

	int _Id = -1;

	public override bool _remove => _bull._remove;

	public void Init(BulletModel aBull) 
	{
		_Id = aBull._id;
		Init();
	}

	public override void UpdateView()
	{
		transform.position = _bull._pos3;

		var ang = transform.localEulerAngles;
		ang.y = _bull._angle;
		transform.localEulerAngles = ang;
	}

	public override bool Return() 
	{
		var ret = base.Return();
		if (ret) _Id = -1;
		return ret;
	}
}
