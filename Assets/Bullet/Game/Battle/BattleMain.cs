using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static BattleConnectionManager;

public class BattleMain : MonoBehaviour
{
	static public BattleMain i { get; set; }

	[SerializeField]
	TextMeshProUGUI _TextTest;

	[SerializeField]
	Transform _DragRoot;

	[SerializeField]
	Transform _CameraRoot;

	[SerializeField]
	Button _BtnQuit;

	[SerializeField]
	int _HistoryInter = 60;

	static public int _historyInter => i._HistoryInter;

	List<BattleBoardModel> _History = new List<BattleBoardModel>();
	static public List<BattleBoardModel> _history => i._History;

	public static Transform _dragRoot => i._DragRoot;

	Console _Console;
	string _Log;

	BattleModel _battle => DataManager._battle;

	BattlePlayerModel _p1 => DataManager._battleP1;
	BattlePlayerModel _p2 => DataManager._battleP2;

	BattlePlayerModel _myPlayer => DataManager._myBattlePlayer;
	BattlePlayerModel _enemyPlayer => DataManager._eneBattlePlayer;

	bool _Init = true;

	EnergyGaugeRootView _EnergyView;
	CountDownView _CountDownView;
	CardRootView _CardRootView;

	UnitViewRoot _UnitViewRoot;
	BulletViewRoot _BullViewRoot;
	HPBarViewRoot _HPBarViewRoot;

	IFrameTween[] _FrameTweens;

	int _frame => _battle._frame;

	void OnValidate()
	{
		name = GetType().Name;
	}

	void Awake()
	{
		i = this;
		_Console = GetComponentInChildren<Console>();
		_EnergyView = GetComponentInChildren<EnergyGaugeRootView>();
		_CountDownView = GetComponentInChildren<CountDownView>();
		_CardRootView = GetComponentInChildren<CardRootView>();
		_UnitViewRoot = GetComponentInChildren<UnitViewRoot>();
		_BullViewRoot = GetComponentInChildren<BulletViewRoot>();
		_HPBarViewRoot = GetComponentInChildren<HPBarViewRoot>();
		_FrameTweens = GetComponentsInChildren<IFrameTween>();
	}

	async void QuitBattle() 
	{
		BattleConnectionCancel();
		await LoadManager.LoadScene("Home");
	}

	async void Start()
	{
		Dir.Init();

		_Console.gameObject.SetActive(false);

		for (int i = 0; i < _FrameTweens.Length; i++)
		{
			_FrameTweens[i].SetAtStart();
		}

		_BtnQuit.onClick.AddListener(QuitBattle);

		await ConnectMasterAsync();

		if (DataManager._reconnect)
		{
			for (int i = 0; i < _FrameTweens.Length; i++)
			{
				_FrameTweens[i].SetAtEnd();
			}
			await WaitBattleModelSyncAsync();
			return;
		}

		_Console.gameObject.SetActive(true);
		_Console.Init("battle>", "_");

		var res = await JoinRoomAsync(0);
		if (res == JoinRoomResult.Fail)
			res = await JoinRoomAsync(1);
		if (res == JoinRoomResult.Fail)
			res = await JoinRoomAsync(2);
		if (res == JoinRoomResult.Fail)
			await CreateAndJoinRoomAsync();

		await WaitPlayerAsync();

		_Log = "init battle\n";
		await _Console.PlayLogAsync(_Log);

		_Log += "       .   .   .   ";
		_Console.PlayLog(_Log);
		
		await WaitBattleStartAsync();

		_Console.gameObject.SetActive(false);
	}

	void FixedUpdate()
	{
		if (_seq != Seq.BattleStarted) return;

		while (true)
		{
			if (DataManager._battle._frame < DataManager._battle._targetFrame)
				Run();
			else
				break;
		}

		_TextTest.text =
			$"_isPlayer1 = {_battle._isP1}\n" +
			$"_myBattlePlayerId = {DataManager._myBattlePlayerId}\n" +
			$"_time = {DataManager._battle._time}\n" +
			$"_targetFrame = {DataManager._battle._targetFrame}\n" +
			$"_Frame = {DataManager._battle._frame}";
	}

	void Run()
	{
		if (_frame == -1)
		{
			Debug.Log("_frame == -1");
			_Init = true;
			_battle._board.SetupTowerAndTurret();
			_battle._frame++;
			return;
		}

		if (_frame % _HistoryInter == 0)
		{
			_History.Add(_battle._board.Copy());
		}

		_battle.ExecCmd(_frame,
			res =>
			{
				_UnitViewRoot.Add(res.unit);
				_HPBarViewRoot.Add(res.unit);
			});

		_p1.IncEnergy();
		_p2.IncEnergy();

		_p1.UnitSearch(true, _p2._units);
		_p2.UnitSearch(false, _p1._units);

		_p1.UnitHitUnit(_p2._units);
		_p2.UnitHitUnit(_p1._units);

		_p1.UnitAct(_battle._board, res => _BullViewRoot.Add(res, true));
		_p2.UnitAct(_battle._board, res => _BullViewRoot.Add(res, false));

		_p1.BullAct(); 
		_p2.BullAct();

		_p1.BullHitEne(_p2._units);
		_p2.BullHitEne(_p1._units);

		_battle._frame++;
	}

	[ContextMenu("InitViewTest")]
	void InitViewTest() 
	{
		i._UnitViewRoot.Clear();
		i._BullViewRoot.Clear();
		i._HPBarViewRoot.Clear();
	}

	void InitView()
	{
		Debug.Log("InitView()");
		i._CardRootView.Clear();
		i._UnitViewRoot.Clear();
		i._BullViewRoot.Clear();
		i._HPBarViewRoot.Clear();

		i._CardRootView.Init(_myPlayer._deck);
		i._UnitViewRoot.Init(i._battle);
		i._BullViewRoot.Init(i._battle);
		i._HPBarViewRoot.Init(i._battle);
	}

	void Update()
	{
		if (_seq != Seq.BattleStarted) return;
		if (_frame < 0) return;

		if (_Init)
		{
			if (!_battle._isP1)
				_CameraRoot.localEulerAngles = new Vector3(0f, 180f, 0f);

			InitView();

			_Init = false;
		}

		_CardRootView.UpdateView();
		_CountDownView.UpdateView(_frame);

		for (int i = 0; i < _FrameTweens.Length; i++)
		{
			_FrameTweens[i].UpdateFrame(_frame);
		}

		_EnergyView.UpdateView(_myPlayer._energyGauge);

		_UnitViewRoot.UpdateView();
		_BullViewRoot.UpdateView();
		_HPBarViewRoot.UpdateView();
	}
}


