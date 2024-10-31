using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class DataManager : Singleton<DataManager>
{

#if UNITY_EDITOR
	[InitializeOnLoadMethod]
	static void InitOnLoad()
	{
		EditorApplication.playModeStateChanged += res =>
		{
			if (res == PlayModeStateChange.EnteredEditMode)
				LoadPlayerModel();
		};
	}
#endif

	[SerializeField]
	PlayerModel _Player;

	BattleModel _Battle;

	string _savePath 
	{
		get 
		{
			var curDir = Directory.GetCurrentDirectory();
			if (Application.isEditor)
				return Path.Join(curDir, "save");

			if (Directory.Exists(Path.Join(curDir, "Assets")))
				return Path.Join(curDir, "build", "save");

			return Path.Join(curDir, "save");
		}
	} 

	public static int _playerId => _player._pId;

	public static BattleBoardModel _battleBoard => i._Battle._board; 
	public static BattlePlayerModel _battleP1 => i._Battle._p1;
	public static BattlePlayerModel _battleP2 => i._Battle._p2;

	public static BattlePlayerModel _myBattlePlayer => _battleP1._pId == _playerId ? _battleP1 : _battleP2;
	public static BattlePlayerModel _eneBattlePlayer => _battleP1._pId == _playerId ? _battleP2 : _battleP1;

	public static int _p1Id => _battleP1._pId;
	public static int _p2Id => _battleP2._pId;

	public static int _myBattlePlayerId => _myBattlePlayer._pId;
	public static int _eneBattlePlayerId => _eneBattlePlayer._pId;

	public static bool _reconnect => PlayerPrefs.HasKey("RoomName");

	public static string _roomName => PlayerPrefs.GetString("RoomName");

	public static PlayerModel _player { get => i._Player; }
	public static BattleModel _battle { get => i._Battle; set => i._Battle = value; }

	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);
		LoadPlayerModel();
		_Player.Init();
	}

	void OnValidate()
	{
		name = GetType().Name;

		Debug.Log($"isPlaying = {Application.isPlaying} frameCount = {Time.frameCount}");

		if (!Application.isPlaying && Time.frameCount != 0)
			SavePlayerModel();
	}

	void OnApplicationQuit()
	{
		SavePlayerModel();
	}

	static public void LoadPlayerModel() 
	{
		Debug.Log("<color=yellow>LoadPlayerModel()</color>");
		if (File.Exists(i._savePath))
			i._Player = JsonUtility.FromJson<PlayerModel>(File.ReadAllText(i._savePath));
		else
		{
			i._Player._pId = Random.Range(10000000, 99999999);
			File.WriteAllText(i._savePath, JsonUtility.ToJson(i._Player));
		}
	}

	static public void SavePlayerModel() 
	{
		Debug.Log("<color=cyan>SavePlayerModelSub()</color>");
		File.WriteAllText(i._savePath, JsonUtility.ToJson(i._Player));
	}

	public static void SaveRoomName(string aRoomName)
	{
		PlayerPrefs.SetString("RoomName", aRoomName);
		PlayerPrefs.Save();
	}

	public static void DeleteRoomName()
	{
		PlayerPrefs.DeleteKey("RoomName");
	}

	[ContextMenu("ResetDeck")]
	void ResetDeck()
	{
		for (int i = 0; i < _Player._decks.Length; i++)
		{
			_Player._decks[i] = new DeckModel();
		}
	}
}