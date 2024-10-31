using System.IO;
using UnityEditor;
using UnityEngine;

public class CurveAsset : ScriptableObject
{
    [SerializeField]
    AnimationCurve _AniCurve;

	public float _time => _AniCurve[_AniCurve.keys.Length - 1].time;

	public float Evaluate(float aTime) => _AniCurve.Evaluate(aTime);

#if UNITY_EDITOR
	[MenuItem("CreateScriptableObject/CurveAsset")]
	static void CreateScriptableObject()
	{
		var obj = ScriptableObject.CreateInstance<CurveAsset>();
		var fileName = $"CurveAsset";
		var ext = ".asset";
		var num = 0;
		var numStr = "";
		var directory = "Assets/ScriptableObject";
		if (!Directory.Exists(directory))
			Directory.CreateDirectory(directory);

		string path;
		while (true)
		{
			if (num != 0)
				numStr = $"({num})";

			path = $"{directory}/{fileName}{numStr}{ext}";
			if (!File.Exists(path))
				break;
			num++;
		}

		AssetDatabase.CreateAsset(obj, path);
	}
#endif
}
