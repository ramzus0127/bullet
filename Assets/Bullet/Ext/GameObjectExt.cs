using UnityEngine;

public static class GameObjectExt
{
	public static T GetOrAddComponent<T>(this GameObject aSelf) 
		where T : Component
	{
		var comp = aSelf.GetComponent<T>();
		if (comp) return comp;
		return aSelf.AddComponent<T>();
	}
}
