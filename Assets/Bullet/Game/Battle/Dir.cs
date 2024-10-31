using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

[Serializable]
public struct Dir
{
	public Dir(float aAngle) 
	{
		_Dir = Mathf.RoundToInt(aAngle / DEG_INTER);
	}

	static Vector2[] gDirs;

	const float DEG_INTER = 360f / 512f;

	[SerializeField]
	int _Dir;

	public static void Init() 
	{
		gDirs = new Vector2[512];

		for (int i = 0; i < 512; i++)
		{
			var deg = DEG_INTER * i;
			var rad = deg * Mathf.Deg2Rad;
			var x = Mathf.Sin(rad);
			var y = Mathf.Cos(rad);
			gDirs[i] = new Vector2(x, y);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetDir(float aDeg)
	{
		_Dir = Mathf.RoundToInt(aDeg / DEG_INTER);
		Validate();
	}

	public void SetDir(Vector2 aTargetPos)
	{
		var angle = GetAngle(aTargetPos);
		_Dir = Mathf.RoundToInt(angle / DEG_INTER);
		Validate();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddDir(float aDeg)
	{
		_Dir += Mathf.RoundToInt(aDeg / DEG_INTER);
		Validate();
	}

	public void MoveTowards(Vector2 aTargetPos, int aDelta) 
	{
		var tage = GetAngle(aTargetPos);
		var angle = Mathf.MoveTowardsAngle(_Dir * DEG_INTER, tage, aDelta * DEG_INTER);
		if (angle < 0) angle += 360f;
		_Dir = Mathf.RoundToInt(angle / DEG_INTER);
		Validate();
	}

	public void MoveTowards(float aAngle, int aDelta)
	{
		var angle = Mathf.MoveTowardsAngle(_Dir * DEG_INTER, aAngle, aDelta * DEG_INTER);
		if (angle < 0) angle += 360f;
		_Dir = Mathf.RoundToInt(angle / DEG_INTER);
		Validate();
	}

	public Vector2 _v2 => gDirs[_Dir];

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float GetAngle(Vector2 aTargetPos)
	{
		var ret = Vector2.Angle(Vector2.up, aTargetPos);

		if (aTargetPos.x < 0)
			return 360f - ret;
		else
			return ret;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	void Validate() 
	{
		while (true)
		{
			if (_Dir >= 512)
			{
				_Dir -= 512;
			}
			else
			{
				break;
			}
		}

		while (true)
		{
			if (_Dir < 0)
			{
				_Dir += 512;
			}
			else
			{ 
				break;
			}
		}
	}

	public float _angle => _Dir * DEG_INTER;

	public static Dir operator +(Dir aSelf, Dir aOther) 
	{
		aSelf._Dir += aOther._Dir;
		aSelf.Validate();
		return aSelf;
	}
}
