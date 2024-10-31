using UnityEngine;

public static class Vector2Ext
{
	public static Vector2 Rotate(this Vector2 aSelf, float aAngle)
	{
		return Quaternion.Euler(0f, 0f, aAngle) * aSelf;
	}
}
