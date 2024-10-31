using System.Collections.Generic;
using UnityEngine;

public abstract class BattleViewRootBase<T> : MonoBehaviour
	where T : BattleViewBase 
{
	protected List<T> _Views = new List<T>();

	void OnValidate()
	{
		name = GetType().Name;
	}

	public void UpdateView() 
	{
		for (int i = 0; i < _Views.Count; i++)
		{
			_Views[i].UpdateView();
		}
		_Views.RemoveAll(a => a.Return());
	}

	public void Clear()
	{
		for (int i = 0; i < _Views.Count; i++)
		{
			_Views[i].ForceReturn();
		}
		_Views.Clear();
	}
}
