using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : Singleton<LoadManager>	
{
	void OnValidate()
	{
		name = GetType().Name;
	}

	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);
	}

	public static async UniTask LoadScene(string aSceneName)
	{
		if (i == null)
			new GameObject("LoadManager", typeof(LoadManager));

		await SceneManager.LoadSceneAsync(aSceneName);
	}
}
