using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static BattleConnectionManager;


public class BattleHomeUI : HomeUIBase
{
	[SerializeField]
    Button _BtnGoBattle;

	[SerializeField]
	Button _BtnCancel;

	[SerializeField]
	TweenPosition _TweenHistory;

	TweenPosition _TweenPosGoBattle;
	TweenPosition _TweenPosCancel;

	Console _Console;

	string _Log;

	void Awake()
	{
		_TweenPosGoBattle = _BtnGoBattle.GetComponent<TweenPosition>();
		_TweenPosCancel = _BtnCancel.GetComponent<TweenPosition>();
		_Console = GetComponentInChildren<Console>();
	}

	void Start()
	{
		_TweenPosGoBattle.SetAtEnd();
		_TweenHistory.SetAtEnd();
		_TweenPosCancel.SetAtStart();
		_BtnGoBattle.onClick.AddListener(() =>
		{
			if (DataManager._player._selectedDeck._isReady)
				Connect();
			else
				Dialog.OpenWithOK("notice", "the deck not ready.\nfill all cards.");
		});
		_BtnCancel.onClick.AddListener(Cancel);
		_Console.Init("battle>", "_");
	}

	async void Connect()
	{
		_TweenPosGoBattle.Rewind();
		_TweenPosCancel.Play();

		_Log = "connect master\n";
		await _Console.PlayLogAsync(_Log);
		_Log += "       .   .   .   ";
		_Console.PlayLog(_Log);
		await ConnectMasterAsync();

		_Log += "success\n";
		_Log += "join room\n";
		await _Console.PlayLogAsync(_Log);

		_Log += "       .   .   .   ";
		_Console.PlayLog(_Log);

		async UniTask onJoinedRoom(JoinRoomResult aJoinRoomResult)
		{
			if (aJoinRoomResult == JoinRoomResult.Player1)
			{
				_Log += "success\n";
				_Log += "       you are first player\n";
				_Log += "       waiting for second player\n";
				await _Console.PlayLogAsync(_Log);
				_Log += "       .   .   .   ";
				_Console.PlayLog(_Log);

				await WaitPlayerAsync();

				_Log += "       second player joined\n";
				await _Console.PlayLogAsync(_Log);
			}

			if (aJoinRoomResult == JoinRoomResult.Player2)
			{
				_Log += "success\n";
				_Log += "       you are second player\n";
				await _Console.PlayLogAsync(_Log);
				await WaitPlayerAsync();
			}
		}

		for (int i = 0; i < 3; i++)
		{
			var res = await JoinRoomAsync(i);

			if (res == JoinRoomResult.Player1 ||
				res == JoinRoomResult.Player2)
			{
				await onJoinedRoom(res);
				break;
			}

			if (res == JoinRoomResult.Fail)
			{
				if (i == 2)
				{
					_Log += "failed\n";
					_Log += "create room\n";
					await _Console.PlayLogAsync(_Log);

					_Log += "       .   .   .   ";
					_Console.PlayLog(_Log);

					var res2 = await CreateAndJoinRoomAsync();

					await onJoinedRoom(res2);
				}
				else
				{
					_Log += "failed\n";
					_Log += "retry\n";
					await _Console.PlayLogAsync(_Log);

					_Log += "       .   .   .   ";
					_Console.PlayLog(_Log);
				}
			}
		}

		_Log += "       load battle? (y/n)\n";
		_Log += "y\n";
		_Log += "       loading battle";
		await _Console.PlayLogAsync(_Log);

		await LoadManager.LoadScene("Battle");
	}

	void Cancel() 
	{
		_TweenPosGoBattle.Play();
		_TweenPosCancel.Rewind();
		_Console.Cancel();
		_Console.ResetLog();
		BattleConnectionCancel();
	}
}

