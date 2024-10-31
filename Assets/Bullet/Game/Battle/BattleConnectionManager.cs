using Cysharp.Threading.Tasks;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using UnityEngine;
using static BattleModel;
using Random = UnityEngine.Random;

public class BattleConnectionManager : MonoBehaviourPunCallbacks
{
	static BattleConnectionManager i { get; set; }

	[SerializeField]
	int _DelayFrame = 20;

	Room _room => PhotonNetwork.CurrentRoom;
	Hashtable _roomCustomProperties => _room.CustomProperties;

	public enum Seq
	{
		None,
		ConnectMaster,
		ConnectedMaster,
		JoinRoom,
		JoinRoomFailed,
		CreateRoom,
		CreateRoomFailed,
		JoinedRoom,
		WaitPlayer,
		AllPlayerJoined,
		WaitBattleStart,
		WaitBattleModelSync,
		BattleStarted,
	}

	Seq _Seq;

	public static Seq _seq => i._Seq;

	public enum JoinRoomResult
	{
		None,
		Player1,
		Player2,
		Fail,
		Skip,
	}

	CancellationTokenSource _Cancel;

	void OnValidate()
	{
		name = GetType().Name;
	}

	void Awake()
	{
		i = this;
		DontDestroyOnLoad(gameObject);
	}

	public static void BattleConnectionCancel()
	{
		PhotonNetwork.Disconnect();
		i._Cancel.Cancel();
		i._Seq = Seq.None;
	}

	void OnDestroy()
	{
		if (_Cancel != null)
			_Cancel.Cancel();
		_Seq = Seq.None;
	}

	[ContextMenu("ResetTime")]
	void ResetTime() 
	{
		var prop = new Hashtable();
		prop["i"] = PhotonNetwork.Time;
		i._room.SetCustomProperties(prop);
	}

	public static async UniTask ConnectMasterAsync() 
	{
		if (i._Seq >= Seq.ConnectedMaster) return;

		i._Seq = Seq.ConnectMaster;
		PhotonNetwork.ConnectUsingSettings();

		i._Cancel = new CancellationTokenSource();
		await UniTask.WaitUntil(() => i._Seq == Seq.ConnectedMaster, cancellationToken: i._Cancel.Token);
	}

	public static async UniTask<JoinRoomResult> CreateAndJoinRoomAsync()
	{
		i._Seq = Seq.CreateRoom;
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.MaxPlayers = 2;
		roomOptions.CustomRoomPropertiesForLobby = new[] { "0", "1", "2", "a" };

		var prop = new Hashtable();
		prop["0"] = DataManager._player._matchingRank100;
		prop["1"] = DataManager._player._matchingRank300;
		prop["2"] = DataManager._player._matchingRank900;
		prop["a"] = true;
		prop["f"] = "a";
		prop["s"] = "a";

		roomOptions.CustomRoomProperties = prop;

		var guid = Guid.NewGuid();
		PhotonNetwork.CreateRoom(guid.ToString(), roomOptions, null);

		return await CreateAndJoinRoomAsyncSub();
	}

	public static async UniTask<JoinRoomResult> JoinRoomAsync(string aRoomName)
	{
		if (i._Seq >= Seq.JoinedRoom) return JoinRoomResult.Skip;
		i._Seq = Seq.JoinRoom;

		PhotonNetwork.JoinRoom(aRoomName);

		return await CreateAndJoinRoomAsyncSub();
	}

	public static async UniTask<JoinRoomResult> JoinRoomAsync(int aRetry) 
	{
		if (i._Seq >= Seq.JoinedRoom) return JoinRoomResult.Skip;
		i._Seq = Seq.JoinRoom;

		Hashtable prop = new Hashtable();
		prop["a"] = true;
		if (aRetry == 0) prop["0"] = DataManager._player._matchingRank100;
		if (aRetry == 1) prop["1"] = DataManager._player._matchingRank300;
		if (aRetry == 2) prop["2"] = DataManager._player._matchingRank900;

		PhotonNetwork.JoinRandomRoom(prop, 0);

		return await CreateAndJoinRoomAsyncSub();
	}

	static async UniTask<JoinRoomResult> CreateAndJoinRoomAsyncSub()
	{
		await UniTask.WaitUntil(() => i._Seq == Seq.JoinedRoom || i._Seq == Seq.JoinRoomFailed || i._Seq == Seq.CreateRoomFailed, cancellationToken: i._Cancel.Token);

		if (i._Seq == Seq.JoinRoomFailed) return JoinRoomResult.Fail;
		if (i._Seq == Seq.CreateRoomFailed) return JoinRoomResult.Fail;

		var prop2 = new Hashtable();
		if (PhotonNetwork.IsMasterClient)
			prop2["f"] = "b";
		else
			prop2["s"] = "b";
		i._room.SetCustomProperties(prop2);

		if (PhotonNetwork.IsMasterClient)
			return JoinRoomResult.Player1;
		else
			return JoinRoomResult.Player2;
	}

	public static async UniTask WaitPlayerAsync() 
	{
		if (i._Seq >= Seq.AllPlayerJoined) return;

		i._Seq = Seq.WaitPlayer;

		i._Cancel = new CancellationTokenSource();
		await UniTask.WaitUntil(
			() => (string)i._roomCustomProperties["f"] == "b" && (string)i._roomCustomProperties["s"] == "b", 
			cancellationToken: i._Cancel.Token);

		var prop2 = new Hashtable();
		prop2["a"] = false;
		i._room.SetCustomProperties(prop2);
		i._Seq = Seq.AllPlayerJoined;
	}

	static void WriteToDst(Stream aSrc, Stream aDst)
	{
		byte[] bytes = new byte[4096];

		int cnt;

		while ((cnt = aSrc.Read(bytes, 0, bytes.Length)) != 0)
		{
			aDst.Write(bytes, 0, cnt);
		}
	}

	static byte[] Compress(string aStr)
	{
		var bytes = Encoding.UTF8.GetBytes(aStr);

		using (var msSrc = new MemoryStream(bytes)) using (var ms = new MemoryStream())
		{
			using (var gZipStream = new GZipStream(ms, CompressionMode.Compress))
			{
				WriteToDst(msSrc, gZipStream);
			}
			return ms.ToArray();
		}
	}

	static string Decompress(byte[] aBytes)
	{
		using (var msSrc = new MemoryStream(aBytes)) using (var ms = new MemoryStream())
		{
			using (var gZipStream = new GZipStream(msSrc, CompressionMode.Decompress))
			{
				WriteToDst(gZipStream, ms);
			}

			return Encoding.UTF8.GetString(ms.ToArray());
		}
	}

	public static async UniTask WaitBattleStartAsync() 
	{
		i._Seq = Seq.WaitBattleStart;

		var prop = new Hashtable();
		var json = JsonUtility.ToJson(new BattlePlayerModel(DataManager._player));
		var zipped = Compress(json);

		if (PhotonNetwork.IsMasterClient)
		{
			var rand = Random.Range(0, int.MaxValue);
			prop["r"] = rand;
			prop["i"] = PhotonNetwork.Time;
			prop["fp"] = zipped;
			//prop["fp"] = json;
		}
		else
		{
			prop["sp"] = zipped;
			//prop["sp"] = json;
		}
		i._room.SetCustomProperties(prop);

		i._Cancel = new CancellationTokenSource();
		await UniTask.WaitUntil(
			() =>
			i._roomCustomProperties.ContainsKey("fp") && 
			i._roomCustomProperties.ContainsKey("sp") &&
			i._roomCustomProperties.ContainsKey("r") &&
			i._roomCustomProperties.ContainsKey("i"),
			cancellationToken: i._Cancel.Token);

		DataManager._battle = new BattleModel();
		DataManager._battle._randSeed = (int)i._roomCustomProperties["r"];
		DataManager._battle._iniTime = (double)i._roomCustomProperties["i"];

		var json1 = Decompress((byte[])i._roomCustomProperties["fp"]);
		var json2 = Decompress((byte[])i._roomCustomProperties["sp"]);

		//var json1 = (string)i._roomCustomProperties["fp"];
		//var json2 = (string)i._roomCustomProperties["sp"];

		Debug.Log(json1);
		Debug.Log(json2);

		DataManager._battle._p1 = JsonUtility.FromJson<BattlePlayerModel>(json1);
		DataManager._battle._p2 = JsonUtility.FromJson<BattlePlayerModel>(json2);

		DataManager._battle.Init();

		DataManager.SaveRoomName(PhotonNetwork.CurrentRoom.Name);

		i._Seq = Seq.BattleStarted;
	}

	public static async UniTask WaitBattleModelSyncAsync()
	{
		i._Seq = Seq.WaitBattleModelSync;

		i._Cancel = new CancellationTokenSource();
		await UniTask.WaitUntil(() => DataManager._battle != null, cancellationToken: i._Cancel.Token);

		i._Seq = Seq.BattleStarted;
	}

	/// <summary>
	/// player 1bit  1
	/// tile   9bit  512
	/// cardid 3bit  8
	/// frame  18bit 262144
	/// </summary>
	public static void SendUseCard(short aTile, byte aCardId) 
	{
		var c = 0;

		var prop = new Hashtable();
		var player1 = DataManager._myBattlePlayer.GetPlayerNum(DataManager._battle) == 1 ? 1 : 0;

		c = AddBit(c, player1, 1);
		c = AddBit(c, aTile, 9);
		c = AddBit(c, aCardId, 3);
		var frame = DataManager._battle._frame + i._DelayFrame;
		c = AddBit(c, frame, 18);
		prop["c"] = c;

		Debug.Log($"<color=yellow>player1 = {player1}</color>");
		Debug.Log($"<color=yellow>aTile = {aTile}</color>");
		Debug.Log($"<color=yellow>aCardId = {aCardId}</color>");
		Debug.Log($"<color=yellow>frame = {frame}</color>");

		i._room.SetCustomProperties(prop);
	}

	static string IntToBitStr(int aVal, int aCnt = 32)
	{
		char[] bits = new char[aCnt];

		for (int i = 0; i < aCnt; i++)
		{
			bits[aCnt - 1 - i] = ((aVal >> i) & 1) == 1 ? '1' : '0';
		}

		return new string(bits);
	}

	static int AddBit(int aDest, int aAdd, int aShiftCnt)
	{
		int shifted = aDest << aShiftCnt;
		return shifted | aAdd;
	}

	static int ExtractBit(int aVal, int aCnt)
	{
		if (aCnt == 0) return 0;
		var mask = 1 << aCnt;
		mask -= 1;
		return aVal & mask;
	}

	public override void OnConnectedToMaster()
	{
		_Seq = Seq.ConnectedMaster;
	}

	public override void OnJoinedRoom()
	{
		_Seq = Seq.JoinedRoom;
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		_Seq = Seq.JoinRoomFailed;
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		_Seq = Seq.JoinRoomFailed;
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		_Seq = Seq.CreateRoomFailed;
	}

	public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
	{
		if (DataManager._battle != null)
		{
			if (propertiesThatChanged.ContainsKey("i"))
				DataManager._battle._iniTime = (double)propertiesThatChanged["i"];

			if (propertiesThatChanged.ContainsKey("c"))
			{
				var c = (int)propertiesThatChanged["c"];
				var frame = ExtractBit(c, 18);
				c = c >> 18;
				var cardId = ExtractBit(c, 3);
				c = c >> 3;
				var tile = ExtractBit(c, 9);
				c = c >> 9;
				var player1 = ExtractBit(c, 1);

				Debug.Log($"<color=lime>player1 = {player1}</color>");
				Debug.Log($"<color=lime>tile = {tile}</color>");
				Debug.Log($"<color=lime>cardId = {cardId}</color>");
				Debug.Log($"<color=lime>frame = {frame}</color>");

				var inter = BattleMain._historyInter;
				var history = BattleMain._history;

				DataManager._battle.AddCmd(new Cmd(player1 == 1 ? true : false, frame, tile, cardId), inter, history);
			}
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		if (DataManager._battle != null)
		{
			var json = JsonUtility.ToJson(DataManager._battle);
			Debug.Log(json);
			photonView.RPC(nameof(RpcReceiveBattleModelJson), RpcTarget.Others, json);
		}
	}

	[PunRPC]
	void RpcReceiveBattleModelJson(string aJson)
	{
		Debug.Log(aJson);
		DataManager._battle = JsonUtility.FromJson<BattleModel>(aJson);
		DataManager._battle.Init();
	}
}


