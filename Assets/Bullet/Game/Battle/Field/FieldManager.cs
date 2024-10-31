using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FieldManager : Singleton<FieldManager>
{
	[SerializeField]
	bool _Update;

	[SerializeField]
    int _X;

	[SerializeField]
	int _Z;

	[SerializeField]
	Transform _TileRoot;

	[SerializeField]
	GameObject _Tile;

	[SerializeField]
	float _TileSize;

	static public float _tileSize => i._TileSize;

	[SerializeField]
	float _TileSizeOffset;

	[SerializeField]
	List<Transform> _TraTiles;

	LineRenderer _LR;

	[SerializeField]
	Material[] _Mats;

	float _x => _X * _TileSize;
	float _z => _Z * _TileSize;

	float _left => -_x / 2f;
	float _right => _x / 2f;
	float _top => _z / 2f;
	float _bottom => -_z / 2f;

	public static Vector3 GetTilePos(int aTileIdx) => i._TraTiles[aTileIdx].position;

	static Vector2 ToV2(Transform aTra) => new Vector2(aTra.position.x, aTra.position.z);
	static Vector3 ToV3(Vector2 aV2) => new Vector3(aV2.x, 0f, aV2.y);

	[SerializeField]
	Transform[] _P1Tower;
	[SerializeField]
	Transform[] _P1TurretL;
	[SerializeField]
	Transform[] _P1TurretR;

	[SerializeField]
	Transform[] _P2Tower;
	[SerializeField]
	Transform[] _P2TurretL;
	[SerializeField]
	Transform[] _P2TurretR;

	public enum TowerPos 
	{
		P1Tower,
		P1TurretL,
		P1TurretR,
		P2Tower,
		P2TurretL,
		P2TurretR,
	}

	protected override void Awake()
	{
		base.Awake();
		if (Application.isPlaying)
		{
			enabled = false;
			return;
		}
		_LR = GetComponentInChildren<LineRenderer>();
	}

	static public Vector2 GetTowerPos(TowerPos aTowerPos) 
	{
		switch (aTowerPos)
		{
			case TowerPos.P1Tower:		return Vector2.Lerp(ToV2(i._P1Tower[0]),	ToV2(i._P1Tower[1]),	0.5f);
			case TowerPos.P1TurretL:	return Vector2.Lerp(ToV2(i._P1TurretL[0]),	ToV2(i._P1TurretL[1]),	0.5f);
			case TowerPos.P1TurretR:	return Vector2.Lerp(ToV2(i._P1TurretR[0]),	ToV2(i._P1TurretR[1]),	0.5f);
			case TowerPos.P2Tower:		return Vector2.Lerp(ToV2(i._P2Tower[0]),	ToV2(i._P2Tower[1]),	0.5f);
			case TowerPos.P2TurretL:	return Vector2.Lerp(ToV2(i._P2TurretL[0]),	ToV2(i._P2TurretL[1]),	0.5f);
			case TowerPos.P2TurretR:	return Vector2.Lerp(ToV2(i._P2TurretR[0]),	ToV2(i._P2TurretR[1]),	0.5f);
		}
		return default; 
	}

	void OnValidate()
	{
		name = GetType().Name;

		for (int i = 0; i < _TraTiles.Count; i++)
		{
			_TraTiles[i].transform.GetChild(0).localScale = Vector3.one * (_TileSize - _TileSizeOffset);
			_TraTiles[i].GetComponent<BoxCollider>().size = new Vector3(_TileSize, _TileSize, 0);
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan; 
		Gizmos.DrawWireCube(ToV3(GetTowerPos(TowerPos.P1Tower)),	Vector3.one * _TileSize * 2);
		Gizmos.DrawWireCube(ToV3(GetTowerPos(TowerPos.P1TurretL)),	Vector3.one * _TileSize * 2);
		Gizmos.DrawWireCube(ToV3(GetTowerPos(TowerPos.P1TurretR)),	Vector3.one * _TileSize * 2);
		Gizmos.DrawWireCube(ToV3(GetTowerPos(TowerPos.P2Tower)),	Vector3.one * _TileSize * 2);
		Gizmos.DrawWireCube(ToV3(GetTowerPos(TowerPos.P2TurretL)),	Vector3.one * _TileSize * 2);
		Gizmos.DrawWireCube(ToV3(GetTowerPos(TowerPos.P2TurretR)),	Vector3.one * _TileSize * 2);
	}

	void Update()
	{
		_LR.positionCount = 5;

		_LR.SetPosition(0, new Vector3(_left, 0, _top));
		_LR.SetPosition(1, new Vector3(_right, 0, _top));
		_LR.SetPosition(2, new Vector3(_right, 0, _bottom));
		_LR.SetPosition(3, new Vector3(_left, 0, _bottom));
		_LR.SetPosition(4, new Vector3(_left, 0, _top));

		if (_Update)
		{
			while (true)
			{
				if (_TileRoot.childCount == 0) break;
				DestroyImmediate(_TileRoot.GetChild(0).gameObject);
			}
			_TraTiles.Clear();
			_Update = false;
		}

		var tileCount = _X * _Z;

		_Tile.SetActive(false);

		if (_TraTiles.Count < tileCount)
		{
			var origin = new Vector3(_left, 0, _bottom);

			for (int i = 0; i < tileCount; i++)
			{
				var tile = Instantiate(_Tile, _TileRoot);

				if (tileCount / 2 > i)
					tile.GetComponentInChildren<Renderer>().sharedMaterial = _Mats[0];
				else
					tile.GetComponentInChildren<Renderer>().sharedMaterial = _Mats[1];

				tile.SetActive(true);
				_TraTiles.Add(tile.transform);
				tile.transform.localPosition = origin + new Vector3((i % _X) * _TileSize + _TileSize / 2, 0, (i / _X) * _TileSize + _TileSize / 2);
				//tile.name = $"X{(i % _X)}.Y{(i / _X)}";
				tile.name = $"{i}";
			}

			for (int i = 0; i < _TraTiles.Count; i++)
			{
				_TraTiles[i].transform.GetChild(0).localScale = Vector3.one * (_TileSize - _TileSizeOffset);
				_TraTiles[i].GetComponent<BoxCollider>().size = new Vector3(_TileSize, _TileSize, 0);
			}
		}
	}
}
