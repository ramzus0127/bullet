using UnityEngine;
using static BattleConnectionManager;

public class Boot : MonoBehaviour
{
	public enum Scene 
	{
		None,
		Home,
		Battle,
	}

	[SerializeField]
	Scene _Scene;

	void OnValidate()
	{
		name = GetType().Name;
	}

	void Awake()
	{
		Application.runInBackground = true;
		Application.targetFrameRate = 60;
	}

	async void Start()
	{
		switch (_Scene)
		{
			case Scene.None:
				if (DataManager._reconnect)
				{
					await ConnectMasterAsync();
					var res = await JoinRoomAsync(DataManager._roomName);
					if (res == JoinRoomResult.Player1 || res == JoinRoomResult.Player2)
					{
						await LoadManager.LoadScene("Battle");
					}
					else
					{
						DataManager.DeleteRoomName();
						BattleConnectionCancel();
						await LoadManager.LoadScene("Home");
					}
				}
				else
				{
					await LoadManager.LoadScene("Home");
				}
				break;
			case Scene.Home:
				await LoadManager.LoadScene("Home");
				break;
			case Scene.Battle:
				await LoadManager.LoadScene("Battle");
				break;
		}
	}
}
