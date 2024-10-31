using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
	[SerializeField]
	CanvasScaler[] _CanvasScalers;

	static Vector2 gScreenSize = new Vector2(800f, 800f / 9f * 16f);

	void OnValidate()
	{
		name = GetType().Name;
	}

	[ContextMenu("Setup")]
	void Setup() 
	{
		_CanvasScalers = GameObject.FindObjectsOfType<CanvasScaler>();

		for (int i = 0; i < _CanvasScalers.Length; i++)
		{
			var canvasScaler = _CanvasScalers[i];

			canvasScaler.matchWidthOrHeight = 1f;
			canvasScaler.referenceResolution = gScreenSize;

			var screenSize = canvasScaler.transform.Find("ScreenSize");
			if (!screenSize)
			{
				var go = new GameObject("ScreenSize");
				go.transform.SetParent(canvasScaler.transform, false);
				screenSize = go.transform;
			}

			var rectTra = screenSize.gameObject.GetOrAddComponent<RectTransform>();

			rectTra.sizeDelta = gScreenSize;
		}
	}
}
