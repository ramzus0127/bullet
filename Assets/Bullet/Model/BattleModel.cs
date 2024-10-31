using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class BattleModel
{
	public BattleModel()
	{
		_Frame = -1;
		_Board = new BattleBoardModel();
	}

	public bool _isP1 => _p1._pId == DataManager._playerId;

	[SerializeField]
	int _RandSeed;
	public int _randSeed { get => _RandSeed; set => _RandSeed = value; }

	[SerializeField]
	double _IniTime;
	public double _iniTime
	{
		get => _IniTime;
		set
		{
			_IniTime = value;
			_Frame = -1;
			_LastTime = 0;
		}
	}

	int _Frame;
	public int _frame { get => _Frame; set => _Frame = value; }

	double _LastTime;

	const double FRAME_TIME = 1d / 60d;

	public int _targetFrame => (int)(_time / FRAME_TIME);

	[Serializable]
	public class Cmd
	{
		public Cmd(bool aIsP1, int aFrame, int aTile, int aCardId) 
		{
			_IsP1 = aIsP1;
			_Frame = aFrame;
			_Tile = aTile;
			_CardId = aCardId;
		}

		[SerializeField]
		bool _IsP1;
		public bool _isP1 => _IsP1;

		[SerializeField]
		int _Frame;
		public int _frame => _Frame;

		[SerializeField]
		int _Tile;
		public int _tile => _Tile;

		[SerializeField]
		int _CardId;
		public int _cardId => _CardId;
		public void Exec(BattleModel aBattle, Action<(bool isP1, UnitModel unit, FieldEffectModel fieldEffect)> aOnExec) 
		{
			Debug.Log($"<color=cyan>player1 = {_IsP1}</color>");
			Debug.Log($"<color=cyan>tile = {_tile}</color>");
			Debug.Log($"<color=cyan>cardId = {_cardId}</color>");
			Debug.Log($"<color=cyan>frame = {_frame}</color>");

			aBattle._board.ExecCard(_cardId, _tile, _IsP1, aOnExec);
		}
	}

	[SerializeField]
	List<Cmd> _Cmds = new List<Cmd>();

	int _CurCmdIdx;

	[SerializeField]
	BattleBoardModel _Board;
	public BattleBoardModel _board  => _Board;

	public BattlePlayerModel _p1 { get => _Board._p1; set => _Board._p1 = value; }
	public BattlePlayerModel _p2 { get => _Board._p2; set => _Board._p2 = value; }

	public double _time
	{
		get
		{
			var deltaTime = PhotonNetwork.Time - _LastTime;
			if (deltaTime < 0)
				_IniTime -= 4294967.295d;
			_LastTime = PhotonNetwork.Time;

			return PhotonNetwork.Time - _IniTime;
		}
	}

	public void Init() 
	{
		_Frame = -1;
		_Board.Init();
	}

	void RollBack(int aTageFrame, int aHistoryInter, List<BattleBoardModel> aHistory) 
	{
		var idx = aTageFrame / aHistoryInter;
		_Frame = idx * aHistoryInter;
		_Board = aHistory[idx].Copy();

		var cmd = _Cmds.First(a => a._frame >= _Frame);
		_CurCmdIdx = _Cmds.IndexOf(cmd);

		aHistory.RemoveRange(idx, aHistory.Count - idx);

		Debug.Log($"rollback to {aTageFrame} {_Frame}");

		//BattleMain.i._Test = true;
	}

	public void AddCmd(Cmd aCmd, int aHistoryInter, List<BattleBoardModel> aHistory)
	{
		if (_Cmds.Count != 0)
		{
			if (_Cmds.Last()._frame > aCmd._frame)
			{
				_Cmds.Add(aCmd);
				_Cmds.OrderBy(a => a._frame);
			}
			else
			{
				_Cmds.Add(aCmd);
			}
		}
		else
		{
			_Cmds.Add(aCmd);
		}

		if (aCmd._frame < _Frame)
			RollBack(aCmd._frame, aHistoryInter, aHistory);
	}

	public void ExecCmd(int aFrame, Action<(bool isPlayer1, UnitModel unit, FieldEffectModel fieldEffect)> aOnExec)
	{
		if (_Cmds.Count == 0) return;
		if (_Cmds.Count < _CurCmdIdx + 1) return;

		var cmd = _Cmds[_CurCmdIdx];

		if (cmd._frame == aFrame)
		{ 
			cmd.Exec(this, aOnExec);
			_CurCmdIdx++;
		}
	}
}

