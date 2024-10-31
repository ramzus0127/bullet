using System;
using UnityEngine;

[Serializable]
public class Coll
{
	[SerializeField]
	Vector2 _Size;

	public Vector3 _size => new Vector3(_Size.x, 0f, _Size.y);

	[SerializeField]
	Vector2 _Offset;

	public bool IsHit(Vector2 aTagePos, Vector2 aSelfPos)
	{
		var left = aSelfPos.x - _Size.x / 2 + _Offset.x;
		var right = aSelfPos.x + _Size.x / 2 + _Offset.x;
		var top = aSelfPos.y + _Size.y / 2 + _Offset.y;
		var bottom = aSelfPos.y - _Size.y / 2 + _Offset.y;

		if (aTagePos.x < left) return false;
		if (right < aTagePos.x) return false;
		if (top < aTagePos.y) return false;
		if (aTagePos.y < bottom) return false;
		return true;
	}

	public bool IsHit(Coll aTageColl, Vector2 aTagePos, Vector2 aSelfPos)
	{
		var left	= aSelfPos.x - _Size.x / 2 + _Offset.x;
		var right	= aSelfPos.x + _Size.x / 2 + _Offset.x;
		var top		= aSelfPos.y + _Size.y / 2 + _Offset.y;
		var bottom	= aSelfPos.y - _Size.y / 2 + _Offset.y;

		var tageLeft	= aTagePos.x - aTageColl._Size.x / 2 + aTageColl._Offset.x;
		var tageRight	= aTagePos.x + aTageColl._Size.x / 2 + aTageColl._Offset.x;
		var tageTop		= aTagePos.y + aTageColl._Size.y / 2 + aTageColl._Offset.y;
		var tageBottom	= aTagePos.y - aTageColl._Size.y / 2 + aTageColl._Offset.y;

		if (tageRight < left) return false;
		if (right < tageLeft) return false;
		if (top < tageBottom) return false;
		if (tageTop < bottom) return false;
		return true;
	}
}
