using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
	[SerializeField]
	GameObject[] _Prefabs;

	Dictionary<int, List<Rent>> _PoolDic = new Dictionary<int, List<Rent>>();

	public Rent Rent(int aTyp)
	{
		GameObject go;
		Rent rent;
		if (!_PoolDic.ContainsKey(aTyp))
			_PoolDic.Add(aTyp, new List<Rent>());

		if (_PoolDic[aTyp].Count == 0)
		{
			go = Instantiate(_Prefabs[(int)aTyp]);
			rent = go.AddComponent<Rent>();
			rent.OnRent(aTyp, this);
		}
		else
		{
			rent = _PoolDic[aTyp][0];
			_PoolDic[aTyp].RemoveAt(0);
		}

		rent.gameObject.SetActive(true);
		return rent;
	}

	public void Return(Rent aReturn) 
	{
		aReturn.gameObject.SetActive(false);
		_PoolDic[aReturn._type].Add(aReturn);
		aReturn.transform.SetParent(transform, false);
	}
}
